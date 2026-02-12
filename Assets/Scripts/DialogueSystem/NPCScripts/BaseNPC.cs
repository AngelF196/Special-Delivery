using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer _interactSprite;
    private Transform _playerTransform;
    private const float INTERACT_DISTANCE = 2.5f;
    private PlayerMove _playerMovementState;

    void Start()
    {
        _playerTransform = GameObject.Find("player").transform;
        _playerMovementState = GameObject.Find("player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_interactKey) && WithinGroundedInteractDistance())
        {
            Interact();
        }

        if (_interactSprite.gameObject.activeSelf && !WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(false);
        }
        else if (!_interactSprite.gameObject.activeSelf && WithinGroundedInteractDistance())
        {
            _interactSprite.gameObject.SetActive(true);
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
