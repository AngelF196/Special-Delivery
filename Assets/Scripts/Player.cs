using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //Changeable variables
    [SerializeField] private float _jump;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _dashPower;
    [SerializeField] private float _maxDashes;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _maxFallSpeed;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;

    //Other
    private Vector2 movement;
    private bool _facingLeft;
    private bool _isGrounded;
    private bool _isDashing;
    private float _originalGravity;
    private float _dashesLeft;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dashesLeft = _maxDashes;
        _originalGravity = _rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Gets player input (needs to change)
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        PlayerMovement(movement);

        //More gravity when falling
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = 3f;
        }
        if (_rb.velocity.y >= 0f)
        {
            _rb.gravityScale = _originalGravity;
        }
    }
    void PlayerMovement(Vector2 direction)
    {
        if (direction.x == Input.GetAxis("Horizontal"))
        {
            _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _horizontalSpeed, _rb.velocity.y);
        }

        //Jump
        if (Input.GetKeyDown("space") && _isGrounded == true)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jump); 
        }

        //Half jumps
        if (Input.GetKeyUp("space") && _rb.velocity.y > 0f) 
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.3f);

        }

        ///Fast fall
        /// if (Input.GetKeyDown("s") &&  _isGrounded == false)
        ///{
        ///    _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y - 50f);
        ///}

        //Cap fall speed
        if (_rb.velocity.y < -50f)
        {
            _rb.velocity = new Vector2 (_rb.velocity.x, _maxFallSpeed);
        }


        if (Input.GetKeyDown("left shift") && _dashesLeft > 0f)
        {
            Debug.Log("shift pressed");
            Dash();
        }

    }
    void Dash() 
    {
        Debug.Log("tried to dash");

        //Performing dash
        _isDashing = true;
        _dashesLeft--;
        _rb.gravityScale = 0f;
        _rb.velocity = movement.normalized * _dashPower;

        //Stopping dash
        DashEnder();
        _rb.gravityScale = _originalGravity;
        _isDashing = false;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
            _dashesLeft = _maxDashes;
            ///_animator.SetBool("grounded", true);
            ///_animator.SetBool("isJumping", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = false;
            ///_animator.SetBool("grounded", false);
            ///_animator.SetBool("isJumping", true);
        }

    }

    IEnumerator DashEnder()
    {
        yield return new WaitForSeconds(_dashTime);
    }
}