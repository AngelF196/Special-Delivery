using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [Header("Player Input")]
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    private Vector2 playerLookDirections;
    [SerializeField] private bool jumpRec; //hide
    [SerializeField] private bool flipActRec; //hide
    [SerializeField] private bool diveActRec; //hide
    [SerializeField] private bool jumpHeld;

    [Space]

    [Header("Input Buffering")]
    [SerializeField] private float inputBuffer;
    private float jumpTimer;
    private float diveTimer;
    private float flipTimer;

    [Space]

    [Header("UI Events")]
    public UnityEvent playerInteract;
    public UnityEvent playerPause;

    //PlayerMove Access
    public bool saysJump => jumpTimer > 0f;
    public bool saysDive => diveTimer > 0f;
    public bool saysFlip => flipTimer > 0f;
    public bool jumpCutRec => !jumpHeld;
    public Vector2 RawDirections => rawPlayerDirections;
    public Vector2 SmoothedDirections => playerDirections;
    public enum Action
    {
        jump,
        dive,
        flip
    }

    //Player Look Access
    public Vector2 look => playerLookDirections;

    void Update()
    {
        if (jumpRec)
        {
            jumpRec = false;
            jumpTimer = inputBuffer;
        }
        if (diveActRec)
        {
            diveActRec = false;
            diveTimer = inputBuffer;
        }
        if (flipActRec)
        {
            flipActRec = false;
            flipTimer = inputBuffer;
        }

        if (jumpTimer > 0f) jumpTimer -= Time.deltaTime;
        if (diveTimer > 0f) diveTimer -= Time.deltaTime;
        if (flipTimer > 0f) flipTimer -= Time.deltaTime;
    }

    public void Consume(Action action)
    {
        switch (action)
        {
            case Action.jump:
                jumpTimer = 0f;
            break;
            case Action.dive:
                diveTimer = 0f;
            break;
            case Action.flip:
                flipTimer = 0f;
            break;
        }
    }

    public void OnJump(InputAction.CallbackContext context) 
    { 
        if (context.started) 
        { 
            jumpRec = true; 
            jumpHeld = true; 
        } 
        else if (context.canceled) 
        { 
            jumpHeld = false; 
        } 
    }
    public void OnFlip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            flipActRec = true;
        }
    }
    public void OnDiveAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            diveActRec = true;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerDirections = context.ReadValue<Vector2>();
            rawPlayerDirections = new Vector2(
                Mathf.Abs(playerDirections.x) > 0.2f ? Mathf.Sign(playerDirections.x) : 0f,
                Mathf.Abs(playerDirections.y) > 0.2f ? Mathf.Sign(playerDirections.y) : 0f
            );
        }
        else if (context.canceled)
        {
            playerDirections = Vector2.zero;
            rawPlayerDirections = Vector2.zero;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerInteract.Invoke();
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerPause.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerLookDirections = context.ReadValue<Vector2>();
            playerLookDirections.Normalize();
        }
        else if (context.canceled)
        {
            playerLookDirections = Vector2.zero;
        }
    }
}
