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

    private void Start()
    {
        _playerTransform = Locator.Instance.Player.gameObject.transform;
        _playerMovementState = Locator.Instance.Player;
        _playerInput = Locator.Instance.Player.gameObject.GetComponent<PlayerInput>();
        _dc = Locator.Instance.DialogueController;
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
        if (_playerMovementState.currentState == PlayerMove.state.grounded && !PauseMenu.gamePaused)
        {
            Interact();
            _playerMovementState.SetRigidBodyVelocity(Vector2.zero);
        }
    }
    
    public abstract void Interact();
}
