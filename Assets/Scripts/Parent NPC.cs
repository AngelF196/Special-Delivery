using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public enum NPCState { Idle, Waving, Interact }

    public NPCState closeState;

    public Sprite idleSprite;
    public Sprite wavingSprite;
    public Sprite interactSprite;
    public SpriteRenderer spriteRenderer;

    public Transform player;

    public float idleRange = 5f;
    public float curiousRange = 3f;
    public float provokedRange = 1f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateState();
        RunActiveState();
    }

    void UpdateState()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= provokedRange)
        {
            closeState = NPCState.Interact;
        }
        else if (distance <= curiousRange)
        {
            closeState = NPCState.Waving;
        }
        else
        {
            closeState = NPCState.Idle;
        }
    }

    void RunActiveState()
    {
        switch (closeState)
        {
            case NPCState.Idle:
                IdleState();
                break;
            case NPCState.Waving:
                WavingState();
                break;
            case NPCState.Interact:
                InteractState();
                break;
        }
    }

    protected virtual void IdleState()
    {
        spriteRenderer.sprite = idleSprite;
    }

    protected virtual void WavingState()
    {
        spriteRenderer.sprite = wavingSprite;
    }

    protected virtual void InteractState()
    {
        spriteRenderer.sprite = interactSprite;
    }
}

