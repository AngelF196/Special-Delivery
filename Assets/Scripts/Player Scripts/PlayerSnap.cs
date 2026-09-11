using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnap : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerMove _playerMove;
    [SerializeField] private float _boostamnt;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerMove = GetComponent<PlayerMove>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Floor Collision")) return;
        if (collision.contactCount > 0)
        {
            Vector2 collisionPoint = collision.contacts[0].point;

            if (collisionPoint.y > transform.position.y) return;
            
            Debug.Log("Valid Floor Collision Detected");

            float pointDiff = transform.position.y - collisionPoint.y;
            if (pointDiff < 0.688 && _playerMove.currentState == PlayerMove.state.midair)
            {
                transform.position = new Vector2(transform.position.x, collisionPoint.y + _boostamnt);
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }

        }
    }


}
