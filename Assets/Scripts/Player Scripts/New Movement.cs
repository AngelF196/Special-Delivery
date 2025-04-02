using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Inputs
    [SerializeField] private bool startJump;
    [SerializeField] private bool startDive;
    [SerializeField] private bool startDash;
    [SerializeField] private bool wallSliding;


    // States
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDiving;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool onGround;
    [SerializeField] private bool onWall;
    [SerializeField] private bool onRightWall;
    [SerializeField] private bool onLeftWall;

    // Changeable variables
    [SerializeField] public float jump;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float maxDashes;

    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashDrag;
    [SerializeField] private float diveDrag;
    [SerializeField] private float diveGravity;
    [SerializeField] private float diveMult;
    [SerializeField] private float divePower;

    // References
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // For Collision
    public LayerMask groundLayer;
    public Vector3 sideBoxes = new Vector3(1, 1, 1);
    public Vector3 bottomBoxes = new Vector3(1, 1, 1);
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    // Other
    public Vector2 movement;
    public Vector2 rawMovement;
    private bool facingLeft;
    private float originalGravity;
    public bool canMove;
    public float walljumpPush;
    public float dashesLeft;
    private bool canDash;
    private Vector2 _dashDirection;

    void Start()
    {
        dashesLeft = maxDashes;
        canDash = true;
        isDiving = false;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravity = _rb.gravityScale;
    }

    void Update()
    {
        // Reset Input
        startDash = false;
        startJump = false;
        startDive = false;

        // Collision
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomBoxes, 0, groundLayer);

        Collider2D colliders = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, bottomBoxes, 0, groundLayer);

        onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, sideBoxes, 0, groundLayer)
            || Physics2D.OverlapBox((Vector2)transform.position + leftOffset, sideBoxes, 0, groundLayer);

        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, sideBoxes, 0, groundLayer);
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, sideBoxes, 0, groundLayer);

        // Get Input
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        startDash = Input.GetKeyDown("x") && dashesLeft > 0f && canDash;
        startJump = Input.GetKeyDown("z") && isDashing == false && isDiving == false;
        startDive = Input.GetKeyDown("c") && onGround == false && isDiving == false;

        // States
        if (isJumping)
        {
            if (Input.GetKeyUp("z") && _rb.velocity.y > 0f && isDiving == false && isDashing == false)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.3f);
            }
        }
        if (isDiving)
        {
            _rb.gravityScale = diveGravity;
            _rb.velocity = new Vector2(_rb.velocity.x + Input.GetAxis("Horizontal") * 0.01f, _rb.velocity.y);
        }
        if (onGround && _rb.velocity.y <= 0)
        {
            isDiving = false;
            isJumping = false;
            dashesLeft = maxDashes;
            _rb.drag = 0f;
        }

        // Movement Applied
        PlayerMovement(movement, rawMovement);

        // Misc
        ///More Gravity When Falling
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = 3f;
        }
        if (_rb.velocity.y > 0f && isDiving == false)
        {
            _rb.gravityScale = originalGravity;
        }

        /// Cap fall speed
        if (_rb.velocity.y < -50f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
        }
    }

    void PlayerMovement(Vector2 direction, Vector2 rawDirection)
    {
        if (isDashing == false && isDiving == false)
        {
            _rb.velocity = new Vector2(direction.x * horizontalSpeed, _rb.velocity.y);
            if (direction.x > 0f)
            {
                facingLeft = false;
                _spriteRenderer.flipX = false;
            }
            if (direction.x < 0f)
            {
                facingLeft = true;
                _spriteRenderer.flipX = true;
            }
        }

        if (startDash)
        {
            Dash();
        }
        if (startJump)
        {
            Jumps();
        }
        if (startDive)
        {
            Dive();
        }
    }

    void Dash()
    {
        Debug.Log("tried to dash");
        _animator.SetBool("isDashing", true);

        //Performing dash
        canDash = false;
        isDashing = true;
        isDiving = false;
        _rb.velocity = Vector2.zero;
        _rb.drag = dashDrag;
        _rb.gravityScale = 0f;
        if (rawMovement.normalized != new Vector2(0, 0))
        {
            _rb.velocity = rawMovement.normalized * dashPower;
        }

        //dash forward if no direction is being held
        if (rawMovement.normalized == new Vector2(0, 0))
        {
            if (_spriteRenderer.flipX == false)
            {
                _rb.velocity = new Vector2(1, 0) * dashPower;
            }
            if (_spriteRenderer.flipX == true)
            {
                _rb.velocity = new Vector2(-1, 0) * dashPower;
            }
        }

        //Stopping dash
        StartCoroutine(DashEnder());

    }
    IEnumerator DashEnder()
    {
        Debug.Log("dash tried to end");
        yield return new WaitForSeconds(0.3f);
        _rb.gravityScale = originalGravity;
        _rb.velocity = new Vector2(_rb.velocity.x * 0.2f, _rb.velocity.y * 0.25f);
        dashesLeft--;
        isDashing = false;
        canDash = true;
        _rb.drag = 0;
        Debug.Log("dash ended");
        _animator.SetBool("isDashing", false);
    }

    void Jumps()
    {
        // Jump
        if (onGround == true)
        {
             _rb.velocity = new Vector2(_rb.velocity.x, 0);
             _rb.velocity += Vector2.up * jump;
             isJumping = true;

             _animator.SetTrigger("Jump");
        }
        
        // Wall jumps
        if (Input.GetKeyDown("z") && onWall && !onGround)
        {
            Debug.Log("walljump");
            if (onRightWall)
            {
                Debug.Log("right wall");
                _rb.velocity = new Vector2(0, jump);
            }
            if (onLeftWall)
            {
                Debug.Log("left wall");
                _rb.velocity = new Vector2(0, jump);
            }
            isJumping = true;
        }
    }
    void Dive()
    {
        isDiving = true;
        _rb.velocity *= diveMult;
        if (_rb.velocity.x > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x + divePower, _rb.velocity.y + divePower);
        }
        if (_rb.velocity.x < 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x - divePower, _rb.velocity.y + divePower);
        }
    }

    void WallSlide()
    {
        if (onRightWall == true && rawMovement.x > 0f)
        {
            Debug.Log("wall slide right");
            _rb.velocity = new Vector2(0, -slideSpeed);
        }
        if (onLeftWall == true && rawMovement.x < 0f)
        {
            Debug.Log("wall slide left");
            _rb.velocity = new Vector2(0, -slideSpeed);
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
}
