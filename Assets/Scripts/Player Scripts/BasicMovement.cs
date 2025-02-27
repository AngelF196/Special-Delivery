using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    //Changeable variables
    [SerializeField] public float _jump;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private DashandDive DnD;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;

    //Other
    public Vector2 movement;
    public Vector2 rawMovement;
    private bool _facingLeft;
    public bool _isGrounded;
    private float _originalGravity;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravity = _rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        PlayerMovement(movement); 

        //More gravity when falling
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = 3f;
        }
        if (_rb.velocity.y > 0f)
        {
            _rb.gravityScale = _originalGravity;
        }
    }
    void PlayerMovement(Vector2 direction)
    {
        if (DnD._isDashing == false && DnD._isDiving == false)
        {
            _rb.velocity = new Vector2(direction.x * _horizontalSpeed, _rb.velocity.y);
        }

        ///if (DnD._isDashing == false && _isGrounded == false)
        ///{
        ///    _rb.velocity = new Vector2(_rb.velocity.y + _horizontalSpeed * 0.06f, _rb.velocity.y);
        ///}

        //Jump
        if (Input.GetKeyDown("z") && _isGrounded == true && DnD._isDashing == false)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += Vector2.up * _jump;
        }

        //Half jumps
        if (Input.GetKeyUp("z") && _rb.velocity.y > 0f && DnD._isDiving == false) 
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


        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
            ///_animator.SetBool("grounded", true);
            ///_animator.SetBool("isJumping", false);
        }

        if (collision.gameObject.tag == "Ground" && DnD._isDiving == true)
        {
            _isGrounded = true;
            DnD._isDiving = false;
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

   
}