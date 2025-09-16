using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private enum state
    {
        grounded, jumping, midair, diving, divelanding, sliding, rolling, walled, boosting, arialboosting
    }

    [Header("Ground Variables")]
    [SerializeField] private float accelRate;
    [SerializeField] private float decelRate;
    private float acceleration;
    [SerializeField] private float maxSpeed;

    [Header("Air Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpcut;
    [SerializeField] private float airspeedmod;

    [Header("Flip/Dive Variables")]
    [SerializeField] private float flipJumpForce;
    [SerializeField] private float diveBoost;
    [SerializeField] private bool hasFlipped; //hide
    [SerializeField] private bool isFlipping; //hide and unused

    [Header("Player Input")]
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    [SerializeField] private state playerState;
    [SerializeField] private bool jumpRec; //hide
    private bool jumpCutRec;
    [SerializeField] private bool flipActRec; //hide
    [SerializeField] private bool diveActRec; //hide

    // References
    private Rigidbody2D _rb;
    private Collider2D _collider;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionlayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftCastDistance;



    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        hasFlipped = false;
    }

    void Update()
    {
        CollisionDetect();
        InputGather();
    }

    private void FixedUpdate()
    {
        //direction and acceleration
        //dive
        //jump
        Action();
    }

    private void InputGather()
    {
        playerDirections = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRec = true;
        }
        jumpCutRec = Input.GetKey(KeyCode.Space) == false && playerState == state.jumping;
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!hasFlipped)
            {
                flipActRec = true;
            }
            else if (hasFlipped)
            {
                diveActRec = true;
            }
        }
            
        
    }

    private bool CollisionDetect()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, collisionlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                float targetSpeed = rawPlayerDirections.x * maxSpeed; //reflects left/right input
                float currentSpeed = _rb.velocity.x;
                acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
                float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
                _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);
                
                //jump and jump state switch
                if (jumpRec)
                {
                    UpdateState(state.jumping);
                    jumpRec = false;
                }

                //dive and dive state switch
                if (diveActRec)
                {
                    UpdateState(state.diving);
                    diveActRec = false;
                }
                break;

            case state.jumping:
                targetSpeed = rawPlayerDirections.x * maxSpeed;
                currentSpeed = _rb.velocity.x;
                acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
                newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * airspeedmod * Time.fixedDeltaTime);
                _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);

                if (jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x,jumpcut * _rb.velocity.y);
                }
                if (flipActRec)
                {
                    airflip();
                }
                if (diveActRec)
                {
                    UpdateState(state.diving);
                }
                if (_rb.velocity.y <= 0f)
                {
                    UpdateState(state.midair);
                }
                if (CollisionDetect())
                {
                    UpdateState(state.grounded);
                }

                //light restricted movement
                //dive and dive state switch
                //jump cut and midair state switch
                //midair state switch
                //walled state switch
                break;
            case state.midair:
                //midair movement
                targetSpeed = rawPlayerDirections.x * maxSpeed; //reflects left/right input
                currentSpeed = _rb.velocity.x;
                acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
                newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * 0.5f * Time.fixedDeltaTime);
                _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }

                if(CollisionDetect())
                {
                    UpdateState(state.grounded);
                }
                if (flipActRec)
                {
                    airflip();
                }
                if (diveActRec)
                {
                    UpdateState(state.diving);
                }

                //light restrited movement
                //grounded state switch
                //walled state switch
                //dive and state switch
                break;
            case state.diving:
                //moderate restricted movement
                //divelanding state switch
                //wall bonk
                break;
            case state.divelanding:
                //hand spring and jump state switch
                //dive boost and midair state switch
                //slide state switch
                //family guy death pose
                break;
            case state.sliding:
                //heavy restricted movement
                //grounded state switch
                //roll and roll state switch
                //neutral getup and grounded state switch
                //wall bonk
                break;
            case state.rolling:
                //grounded state switch
                //jump and jump state switch
                //wall bonk
                break;
            case state.walled:
                //wall run
                //wall slide
                //wall jump and jump state switch
                //midair state switch
                break;

            case state.boosting:

                break;
            case state.arialboosting:

                break;
        }
    }

    private void airflip() 
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * flipJumpForce, ForceMode2D.Impulse);
        
        Debug.Log("tried to flip");
        hasFlipped = true;
        flipActRec = false;
    }

    private void airdive()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        float forwardSpeed = Mathf.Abs(_rb.velocity.x);

        _rb.AddForce(new Vector2(direction * (forwardSpeed + diveBoost), 0f), ForceMode2D.Impulse);
        diveActRec = false;
    }

    private void UpdateState(state newstate)
    {
        Debug.Log(newstate.ToString() + " state");
        playerState = newstate;
        StateAction(newstate);
    }

    private void StateAction(state newstate)
    {
        switch (newstate)
        {
            case state.jumping:
                //jump
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                break;
            case state.grounded:
                hasFlipped = false;
                break;
            case state.diving:
                airdive();
                break;

        }
    }
}
