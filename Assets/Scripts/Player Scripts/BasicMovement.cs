using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    // Changeable variables
    [SerializeField] public float _jump;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _maxFallSpeed;

    // References
    private Rigidbody2D _rb;
    private Animator _animator;  // Added Animator reference
    private SpriteRenderer _spriteRenderer;  // Added SpriteRenderer reference
    public PlayerData Data;
    [SerializeField] private DashandDive DnD;

    // Other
    public Vector2 movement;
    public Vector2 rawMovement;
    private bool _facingLeft;
    public bool _isGrounded;
    private float _originalGravity;
    public bool canMove;
    public float slideSpeed;
    public bool _isJumping;

    // For Collision
    public LayerMask groundLayer;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public Vector3 sideBoxes = new Vector3(1, 1, 1);
    public Vector3 bottomBoxes = new Vector3(1, 1, 1);

    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>(); 
        _spriteRenderer = GetComponent<SpriteRenderer>();  
        _originalGravity = _rb.gravityScale;
    }

    void Update()
    {
        // Get Input + Movement Function
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        PlayerMovement(movement);

        // Update Animator isRunning based on movement
        _animator.SetBool("isRunning", Mathf.Abs(movement.x) > 0);

        // Flip the sprite based on direction
        if (movement.x > 0 && _spriteRenderer.flipX)  // Moving right
        {
            _spriteRenderer.flipX = false;
        }
        else if (movement.x < 0 && !_spriteRenderer.flipX)  // Moving left
        {
            _spriteRenderer.flipX = true;
        }

        // More gravity when falling
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = 3f;
        }
        if (_rb.velocity.y > 0f && DnD._isDiving == false)
        {
            _rb.gravityScale = _originalGravity;
        }

        // Cap fall speed
        if (_rb.velocity.y < -50f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _maxFallSpeed);
        }

        // Collision
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomBoxes, 0, groundLayer);

        Collider2D colliders = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomBoxes, 0, groundLayer);

        onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, sideBoxes, 0, groundLayer)
            || Physics2D.OverlapBox((Vector2)transform.position + leftOffset, sideBoxes, 0, groundLayer);

        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, sideBoxes, 0, groundLayer);
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, sideBoxes, 0, groundLayer);

        wallSide = onRightWall ? -1 : 1;


        if (onGround)
        {
            _isJumping = false;
        }

        //Wall slide
        if (rawMovement.x != 0f && onWall && !onGround) 
        {
            WallSlide();
        }
    }

    void PlayerMovement(Vector2 direction)
    {
        if (DnD._isDashing == false && DnD._isDiving == false)
        {
            _rb.velocity = new Vector2(direction.x * _horizontalSpeed, _rb.velocity.y);
        }

        Jumps();
    }

    void Jumps()
    {
        // Jump
        if (Input.GetKeyDown("z") && DnD._isDashing == false)
        {
            if (onGround == true)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.velocity += Vector2.up * _jump;
                _isJumping = true;
            }
        }

        // Half jumps
        if (Input.GetKeyUp("z") && _rb.velocity.y > 0f && DnD._isDiving == false && _isJumping == false)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.3f);
        }

        // Wall jumps
        if (Input.GetKeyDown("z") && onWall && !onGround)
        {
            Vector2 wallDir = onRightWall ? Vector2.left : Vector2.right;
            _rb.velocity += (Vector2.up + wallDir) * _jump;
            _isJumping = true;
        }
    }

    void WallSlide()
    {
        if (onRightWall == true && rawMovement.x > 0f)
        {
            _rb.velocity = new Vector2(0, -slideSpeed);
        }
        if (onLeftWall == true && rawMovement.x < 0f)
        {
            _rb.velocity = new Vector2(0, -slideSpeed);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && DnD._isDiving == true)
        {
            _isGrounded = true;
            DnD._isDiving = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, bottomBoxes);
        Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, sideBoxes);
        Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, sideBoxes);
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
