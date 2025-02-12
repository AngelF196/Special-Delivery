using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //Changeable variables
    [SerializeField] private int _jump;
    [SerializeField] private int _horizontalSpeed;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;

    //Other
    private Vector2 movement;
    private bool _facingLeft;
    private bool _isGrounded;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        PlayerMovement(movement);

        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = 3;
        }
        if (_rb.velocity.y > 0)
        {
            _rb.gravityScale = 1;
        }
    }
    void PlayerMovement(Vector2 direction)
    {
        if (direction.x == Input.GetAxis("Horizontal"))
        {
            _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _horizontalSpeed, _rb.velocity.y);
        }

        if (Input.GetKeyDown("space"))
        {
            if (_isGrounded == true) 
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jump); 
            }
        }

        if (Input.GetKeyDown("left shift"))
        {
            Dash(direction);
        }

    }
    void Dash(Vector2 direction) 
    { 
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
            //_animator.SetBool("grounded", true);
            //_animator.SetBool("isJumping", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = false;
            //_animator.SetBool("grounded", false);
            //_animator.SetBool("isJumping", true);
        }

    }
}