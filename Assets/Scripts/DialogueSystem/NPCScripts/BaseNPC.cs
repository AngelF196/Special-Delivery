using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer _interactSprite;
    private const float INTERACT_DISTANCE = 2.5f;
    private Transform _playerTransform;
    private PlayerMove _playerMovementState;
    private DialogueController _dc;

    // Flag for checking if the player went out of range while in the middle of conversation
    private bool _goneOutOfRange = true;

    private void Start()
    {
        _playerTransform = GameObject.Find("player").transform;
        _playerMovementState = GameObject.Find("player").GetComponent<PlayerMove>();
        _dc = GameObject.Find("DialogueSystem").transform.GetChild(0).GetComponent<DialogueController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_interactSprite.gameObject.activeSelf && !WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(false);
        }
        else if (!_interactSprite.gameObject.activeSelf && WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(_interactKey) && WithinGroundedInteractDistance())
        {
            Interact();

            if (_goneOutOfRange)
            {
                _goneOutOfRange = false;
                Debug.Log("Changed out of NPC interact range condition: " + _goneOutOfRange);
            }
        }
        else if (!WithinGroundedInteractDistance())
        {
            if (!_goneOutOfRange)
            {
                _goneOutOfRange = true;
                Debug.Log("Changed out of NPC interact range condition: " + _goneOutOfRange);
                _dc.ExitConversationAfterStrayingFromNPC();
            }
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
    
    public abstract void Interact();
}
