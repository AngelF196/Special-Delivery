using System;
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
    [SerializeField] private float handspringMult;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpcut;
    [SerializeField] private float airspeedmod;
    [SerializeField] private float jumpBuffer; 


    [Header("Flip/Dive Variables")]
    [SerializeField] private float flipJumpForce;
    [SerializeField] private float diveBoost;
    [SerializeField] private bool hasFlipped; //hide
    [SerializeField] private bool isFlipping; //hide and unused
    [SerializeField] private float diveLandMaxTime;
    [SerializeField] private float diveSpringHeight;
    [SerializeField] private float diveSpringLength;
    [SerializeField] private float divespeedmod;

    [Header("Player Input")]
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    [SerializeField] private state playerState;
    [SerializeField] private bool jumpRec; //hide
    private bool jumpCutRec;
    [SerializeField] private bool flipActRec; //hide
    [SerializeField] private bool diveActRec; //hide
    [SerializeField] private bool facingLeft; //hide

    [Header("Collision")]
    [SerializeField] private LayerMask collisionlayer;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftCastDistance;

    // References
    private Rigidbody2D _rb;
    private Collider2D _collider;

    //misc shit
    private float jumpTimer;
    private float diveLandTimer;
    private state prevState;
    private float storedSpeed;
    [Header("Boost")]
    [SerializeField] private int boostStage;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        hasFlipped = false;
        jumpTimer = jumpBuffer;
        diveLandTimer = diveLandMaxTime;
    }

    void Update()
    {
        FloorDetect();
        InputGather();
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            DirectionFacing();
        }
    }

    private void FixedUpdate()
    {
        Action();

        if (jumpRec)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0)
            {
                jumpRec = false;
                jumpTimer = jumpBuffer;
            }
        }
        if (playerState == state.divelanding)
        {
            diveLandTimer -= Time.deltaTime;

            if (diveLandTimer <= 0)
            {
                UpdateState(state.grounded);
                diveLandTimer = diveLandMaxTime;
            }
        }

    }

    private void InputGather()
    {
        playerDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rawPlayerDirections = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRec = true;
        }
        jumpCutRec = Input.GetKey(KeyCode.Space) == false && playerState == state.jumping;
        if (Input.GetKeyDown(KeyCode.LeftShift))
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

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                MovementCalc();

                if (jumpRec)
                {
                    UpdateState(state.jumping);
                }
                if (FloorDetect() == false) //Covers if going straight from ground to airborne
                {
                    if (_rb.velocity.y > 0)
                    {
                        UpdateState(state.jumping);
                    }
                    else
                    {
                        UpdateState(state.midair);
                    }
                }
                if (diveActRec || flipActRec)
                {
                    UpdateState(state.diving);
                    diveActRec = false;
                    flipActRec = false;
                }
                break;
            case state.jumping:
                MovementCalc();

                if (jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpcut * _rb.velocity.y);
                }
                if (flipActRec)
                {
                    AirFlip();
                }
                if (diveActRec)
                {
                    UpdateState(state.diving);
                }
                if (_rb.velocity.y <= 0f)
                {
                    UpdateState(state.midair);
                }
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                
                //walled state switch
                break;
            case state.midair:
                MovementCalc();

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (flipActRec)
                {
                    AirFlip();
                }
                if (diveActRec)
                {
                    UpdateState(state.diving);
                }

                //walled state switch
                break;
            case state.diving:
                MovementCalc();

                if (FloorDetect())
                {
                    UpdateState(state.divelanding);
                }

                //wall bonk
                break;
            case state.divelanding:
                if (flipActRec || diveActRec)
                {
                    UpdateState(state.boosting);
                }
                if (jumpRec)
                {
                    UpdateState(state.jumping);
                }

                //family guy death pose
                break;
            case state.boosting:
                MovementCalc();

                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (flipActRec)
                {
                    AirFlip();
                }
                break;
            case state.walled:
                //wall run
                //wall slide
                //wall jump and jump state switch
                //midair state switch
                break;
        }
    }
    
    private void UpdateState(state newstate)
    {
        Debug.Log(newstate.ToString() + " state");
        prevState = playerState;
        playerState = newstate;
        StateAction(newstate);
    }
    private void StateAction(state newstate)
    {
        switch (newstate)
        {
            case state.jumping:
                if(prevState == state.divelanding)
                {
                    _rb.velocity = new Vector2(storedSpeed, handspringMult * jumpForce * Time.fixedDeltaTime); //hand spring
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime); //jump
                }
                jumpRec = false;
                break;
            case state.grounded:
                hasFlipped = false;
                break;
            case state.divelanding:
                hasFlipped = false;
                storedSpeed = _rb.velocity.x;
                _rb.velocity = new Vector2(0f, _rb.velocity.y);
                break;
            case state.diving:
                Dive();
                break;
            case state.boosting:
                boostStage += 1;
                DiveSpringBoost();
                break;

        }
    }
    private void DirectionFacing()
        {
        if (rawPlayerDirections.x == -1)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
        }

    private void MovementCalc()
    {
        float targetSpeed = rawPlayerDirections.x * maxSpeed; //reflects left/right input
        float currentSpeed = _rb.velocity.x;
        acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
        float newSpeed = 0;
        if (playerState == state.grounded)
        {
            newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime); //unrestricted movement
        }
        else if (playerState == state.jumping || playerState == state.midair)
        {
            newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * airspeedmod * Time.fixedDeltaTime); //light restricted movement
        }
        else if (playerState == state.diving || playerState == state.boosting)
        {
            newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * divespeedmod * Time.fixedDeltaTime); //heavy restricted movement
        }
        _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);
    }
    private void AirFlip()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * flipJumpForce, ForceMode2D.Impulse);

        hasFlipped = true;
        flipActRec = false;
    }
    private void Dive()
    {
        float forwardSpeed = Mathf.Abs(_rb.velocity.x);
        if (playerState != state.grounded) 
        {
            if (facingLeft)
            {
                int direction = -1;
                _rb.AddForce(new Vector2((direction * (forwardSpeed + diveBoost)), 0f), ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(new Vector2((forwardSpeed + diveBoost), 0f), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (facingLeft)
            {
                int direction = -1;
                _rb.AddForce(new Vector2((direction * (forwardSpeed + diveBoost)), 0f), ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(new Vector2((forwardSpeed + diveBoost), 0f), ForceMode2D.Impulse);
            }
        }
        diveActRec = false;
    }

    private void DiveSpringBoost()
    {
        float forwardSpeed = Mathf.Abs(_rb.velocity.x);
        if (facingLeft)
        {
            int direction = -1;
            _rb.velocity = new Vector2(direction * diveSpringLength, diveSpringHeight);
        }
        else
        {
            _rb.velocity = new Vector2(diveSpringLength, diveSpringHeight);
        }
        flipActRec = false;
        diveActRec = false;
    }

    private bool FloorDetect()
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
}
