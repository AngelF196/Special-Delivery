using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer _interactSprite;
    private const float INTERACT_DISTANCE = 2.5f;
    private Transform _playerTransform;
    private PlayerMove _playerMovementState;
    private PlayerInput _playerInput;
    private DialogueController _dc;
    // Flag for checking if the player went out of range while in the middle of conversation
    private bool _goneOutOfRange = true;

    private void Start()
    {
        _playerTransform = GameObject.Find("player").transform;
        _playerMovementState = GameObject.Find("player").GetComponent<PlayerMove>();
        _playerInput = GameObject.Find("player").GetComponent<PlayerInput>();
        _dc = GameObject.Find("DialogueSystem").transform.GetChild(0).GetComponent<DialogueController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_interactSprite.gameObject.activeSelf && !WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(false);
            _playerInput.playerInteract.RemoveListener(respondToInteract);

        }
        else if (!_interactSprite.gameObject.activeSelf && WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(true);
            _playerInput.playerInteract.AddListener(respondToInteract);

        }
    }

    private bool WithinGroundedInteractDistance()
    {
        if (_playerMovementState.currentState == PlayerMove.state.grounded && Vector2.Distance(_playerTransform.position, transform.position) < INTERACT_DISTANCE)
        {
            return true;
        }
        return false;
    }

    public void respondToInteract()
    {
        if (_playerMovementState.currentState == PlayerMove.state.grounded)
        {
            Interact();
            _playerMovementState.SetRigidBodyVelocity(Vector2.zero);
        }
    }
    
    public abstract void Interact();
}
