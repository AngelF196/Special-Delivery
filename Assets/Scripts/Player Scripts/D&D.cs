using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DashandDive : MonoBehaviour
{
    [SerializeField] private float _dashPower;
    [SerializeField] private float _maxDashes;
    [SerializeField] private float _dashTime;
    [SerializeField] private BasicMovement _PlayerMove;

    //References
    private Rigidbody2D _rb;
    public PlayerData Data;

    //Other
    private Vector2 movement;
    private bool _facingLeft;
    private bool _isGrounded;
    private float _originalGravity;
    private bool _isDashing;
    private float _dashesLeft;

    // Start is called before the first frame update
    void Start()
    {
        _dashesLeft = _maxDashes;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left shift") && _dashesLeft > 0f)
        {
            Debug.Log("shift pressed");
            Dash();
        }

        if (_isDashing)
        {

        }

    }
    void Dash()
    {
        Debug.Log("tried to dash");

        //Performing dash
        _isDashing = true;
        _dashesLeft--;
        _rb.gravityScale = 0f;
        _rb.velocity = Vector2.zero;
        _rb.velocity = _PlayerMove.movement.normalized * _dashPower;

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
    {
        Debug.Log("dash tried to end");
        yield return new WaitForSeconds(0.3f);
        Debug.Log("dash ended");
        _rb.gravityScale = 3;
        _isDashing = false;

    }
}
