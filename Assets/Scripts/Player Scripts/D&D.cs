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
    [SerializeField] private BasicMovement _PlayerMove;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;

    //Other
    private Vector2 movement;
    private bool _facingLeft;
    private bool _isGrounded;
    private float _originalGravity;
    public bool _isDashing;
    public float _dashesLeft;
    private bool _canDash;


    // Start is called before the first frame update
    void Start()
    {
        _dashesLeft = _maxDashes;
        _canDash = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left shift") && _dashesLeft > 0f && _canDash)
        {
            Debug.Log("shift pressed");
            Dash();
        }

        if (_isDashing)
        {
            _rb.drag = _dashDrag;
            _rb.gravityScale = 0f;
        }
        if (_isDashing == false)
        {
            _rb.drag = 0f;
        }

    }
    void Dash()
    {
        Debug.Log("tried to dash");

        //Performing dash
        _isDashing = true;
        _dashesLeft--;
        _rb.velocity = Vector2.zero;
        _rb.velocity = _PlayerMove.rawMovement.normalized * _dashPower;

        //Stopping dash
        StartCoroutine(DashEnder());

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _dashesLeft = _maxDashes;
            ///_animator.SetBool("grounded", true);
            ///_animator.SetBool("isJumping", false);
        }
    }

    IEnumerator DashEnder()
    {;
        Debug.Log("dash tried to end");
        yield return new WaitForSeconds(0.3f);
        _rb.gravityScale = 3;
        _rb.velocity = new Vector2 (_rb.velocity.x * 0.2f, _rb.velocity.y * 0.3f);
        _isDashing = false;
        _canDash = true;
        Debug.Log("dash ended");

    }
}
