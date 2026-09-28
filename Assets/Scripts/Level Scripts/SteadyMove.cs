using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteadyMove : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private Vector2 _direction;
    private Rigidbody2D _rb;
    private PlayerMove _player;
    private Vector2 _prevPosition = Vector2.zero;
    private Vector2 _currentPosition = Vector2.zero;
    private Vector2 _deltaPosition = Vector2.zero;
    [SerializeField] private Vector2 _platformVelocity = Vector2.zero;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentPosition = transform.position;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _velocity * Time.fixedDeltaTime);

        _prevPosition = _currentPosition;
        _currentPosition = _rb.position;

        _deltaPosition = _currentPosition - _prevPosition;
        _platformVelocity = _deltaPosition/Time.fixedDeltaTime;
        if (_player != null)
        {
            _player.SetPlatformVelocity(_platformVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player is on top of the platform
            if (collision.contacts[0].normal.y <= -0.7f)
            {
                _player = collision.gameObject.GetComponent<PlayerMove>();

                // Transfer velocity only if the player knows they're grounded
                if (_player.currentState == PlayerMove.state.grounded)
                {
                    _player.SetRigidBodyVelocity(_player.GetComponent<Rigidbody2D>().velocity + _platformVelocity);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _player is not null)
        {
            _player.SetPlatformVelocity(Vector2.zero);
            _player = null;
        }
    }
}
