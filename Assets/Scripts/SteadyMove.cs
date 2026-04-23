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
    private Vector2 _platformVelocity = Vector2.zero;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentPosition = transform.position;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _velocity * Time.fixedDeltaTime);

        _prevPosition = _currentPosition;
        _currentPosition = transform.position;

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
            _player = collision.gameObject.GetComponent<PlayerMove>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player.SetPlatformVelocity(Vector2.zero);
            _player = null;
        }
    }
}
