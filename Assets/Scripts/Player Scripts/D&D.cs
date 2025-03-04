using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DashandDive : MonoBehaviour
{
    [SerializeField] private float _dashPower;
    [SerializeField] private float _maxDashes;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashDrag;
    [SerializeField] private float _diveDrag;
    [SerializeField] private float _diveGravity;
    [SerializeField] private float _diveMult;
    [SerializeField] private float _divePower;
    [SerializeField] private BasicMovement _PlayerMove;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;
    private SpriteRenderer _spriteRenderer;

    //Other
    private bool _facingLeft;
    private float _originalGravity;
    public bool _isDashing;
    public float _dashesLeft;
    private bool _canDash;
    public bool _isDiving;
    private Vector2 _dashDirection;


    // Start is called before the first frame update
    void Start()
    {
        _dashesLeft = _maxDashes;
        _canDash = true;
        _isDiving = false;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x") && _dashesLeft > 0f && _canDash)
        {
            Debug.Log("dash pressed");
            Dash();
        }
        if (Input.GetKeyDown("c") && _PlayerMove.onGround == false && _isDiving == false)
        {
            Dive();
        }
        if (_isDiving && _PlayerMove.onGround && Input.GetKeyDown("z"))
        {
            Debug.Log("Tried to wavedash");
            Wavedash();
        }

        if (_isDiving)
        {
            _rb.gravityScale = _diveGravity;
            _rb.velocity = new Vector2(_rb.velocity.x + Input.GetAxis("Horizontal") * 0.01f, _rb.velocity.y);
        }

        if (_isDashing)
        {
            _rb.drag = _dashDrag;
            _rb.gravityScale = 0f;
        }
        if (_isDashing == false && _isDiving == false)
        {
            _rb.drag = 0f;
        }

        if (_PlayerMove.onGround)
        {
            _dashesLeft = _maxDashes;
        }
    }

    void Wavedash()
    {
        Debug.Log("Wavedash");
        _dashDirection = _rb.velocity.normalized;
        _rb.velocity = Vector2.zero;
        _isDashing = false;
        _canDash = true;
        _rb.velocity += Vector2.up * _PlayerMove._jump;
        _rb.AddForce(_dashDirection * _dashPower);
    }
    void Dive()
    {
        _isDiving = true;
        _rb.velocity *= _diveMult;
        if (_rb.velocity.x > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x + _divePower, _rb.velocity.y + _divePower);
        }
        if (_rb.velocity.x < 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x - _divePower, _rb.velocity.y + _divePower);
        }
    }
    void Dash()
    {
        Debug.Log("tried to dash");

        //Performing dash
        _isDashing = true;
        _isDiving = false;
        _rb.velocity = Vector2.zero;
        if (_PlayerMove.rawMovement.normalized != new Vector2 (0,0))
        {
            _rb.velocity = _PlayerMove.rawMovement.normalized * _dashPower;

        }

        //dash forward if no direction is being held
        if (_PlayerMove.rawMovement.normalized == new Vector2(0, 0))
        {
            if (_spriteRenderer.flipX == false)
            {
                _rb.velocity = new Vector2(1, 0) * _dashPower;

            }
            if (_spriteRenderer.flipX == true) 
            {
                _rb.velocity = new Vector2 (-1,0) * _dashPower;

            }
        }

        //Stopping dash
        StartCoroutine(DashEnder());

    }
    IEnumerator DashEnder()
    {;
        Debug.Log("dash tried to end");
        yield return new WaitForSeconds(0.3f);
        _rb.gravityScale = 3;
        _rb.velocity = new Vector2 (_rb.velocity.x * 0.2f, _rb.velocity.y * 0.25f);
        _dashesLeft--;
        _isDashing = false;
        _canDash = true;
        Debug.Log("dash ended");

    }
}
