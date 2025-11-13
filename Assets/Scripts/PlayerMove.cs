using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private enum state
    {
        grounded, jumping, midair, diving, divelanding, walled, boosting
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
    [SerializeField] private float diveLandMaxTime;
    [SerializeField] private float diveSpringHeight;
    [SerializeField] private float diveSpringLength;
    [SerializeField] private float divespeedmod;
    [SerializeField] private float diveYbump;


    [Header("Wall Variables")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpXForce;
    [SerializeField] private float wallJumpMult;
    [SerializeField] private bool DetectWalls;
    private float wallTimer;
    [SerializeField] private float wallBuffer;
    [SerializeField] private float wallDashForce;
    [SerializeField] private bool hasWallDashed;



    [Header("Player Input")]
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    [SerializeField] private state playerState;
    [SerializeField] private bool jumpRec; //hide
    private bool jumpCutRec;//hide
    [SerializeField] private bool flipActRec; //hide
    [SerializeField] private bool diveActRec; //hide
    [SerializeField] private bool facingLeft; //hide

    [Header("Collision")]
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector2 wallBoxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftCastDistance;

    // References
    private Rigidbody2D _rb;
    private Collider2D _collider;

    //misc shit
    private float jumpTimer;
    private float diveTimer;
    private float flipTimer;
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
        diveTimer = jumpBuffer;
        diveLandTimer = diveLandMaxTime;
        boostStage = 0;
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
            jumpTimer -= Time.fixedDeltaTime;

            if (jumpTimer <= 0)
            {
                jumpRec = false;
                jumpTimer = jumpBuffer;
            }
        }
        if (diveActRec)
        {
            diveTimer -= Time.fixedDeltaTime;

            if (diveTimer <= 0)
            {
                diveActRec = false;
                diveTimer = jumpBuffer;
            }
        }
        if (flipActRec)
        {
            flipTimer -= Time.fixedDeltaTime;

            if (flipTimer <= 0)
            {
                flipActRec = false;
                flipTimer = jumpBuffer;
            }
        }
        if (playerState == state.divelanding)
        {
            diveLandTimer -= Time.fixedDeltaTime;

            if (diveLandTimer <= 0)
            {
                UpdateState(state.grounded);
                diveLandTimer = diveLandMaxTime;
            }
        }
        if (!DetectWalls)
        {
            wallTimer -= Time.fixedDeltaTime;

            if (wallTimer <= 0)
            {
                DetectWalls = true;
                wallTimer = wallBuffer;
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
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
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
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
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
                if (WallDirectionDetect() != 0 && WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                    break;
            case state.walled:
                WallMovement();

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }
                if (WallDirectionDetect() == 0)
                {
                    UpdateState(state.midair);
                }
                if (FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (jumpRec)
                {
                    UpdateState(state.jumping);
                }
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
                    if (MathF.Sign(rawPlayerDirections.x) != MathF.Sign(storedSpeed)) 
                    {
                        _rb.velocity = new Vector2(storedSpeed/2, handspringMult * jumpForce * Time.fixedDeltaTime); //hand spring
                    }
                    else
                    {
                        _rb.velocity = new Vector2(storedSpeed, handspringMult * jumpForce * Time.fixedDeltaTime); //hand spring
                    }
                }
                else if (prevState == state.walled)
                {
                    _rb.velocity = new Vector2(-WallDirectionDetect() * wallJumpXForce * Time.fixedDeltaTime, wallJumpMult * jumpForce * Time.fixedDeltaTime); //wall jump
                }
                else if (prevState == state.midair)
                {
                    DetectWalls = true;
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime); //jump
                }

                jumpRec = false;
                hasWallDashed = false;
                break;
            case state.grounded:
                hasFlipped = false;
                hasWallDashed = false;
                break;
            case state.divelanding:
                hasFlipped = false;
                hasWallDashed = false;
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
            case state.walled:
                hasFlipped = false;
                if (prevState != state.walled)
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case state.midair:
                DetectWalls = true;
                break;
        }
        if (prevState == state.walled)
        {
            DetectWalls = false;
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
        if (playerState == state.grounded || playerState == state.walled)
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
    private void WallMovement()
    {
        if ((flipActRec || diveActRec) && !hasWallDashed) //wall dash
        {
            _rb.velocity = new Vector2(0, wallDashForce);
            hasWallDashed = true;
            flipActRec = false;
            diveActRec = false;
        }
        else if (flipActRec || diveActRec)
        {
            flipActRec = false;
            diveActRec = false;
        }
        if (WallDirectionDetect() == rawPlayerDirections.x && _rb.velocity.y < 0) //wall slide
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else //move away from wall
        {
            MovementCalc();
        }
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
        float divespeed = Mathf.Clamp(forwardSpeed, 5f, maxSpeed);
        if (prevState != state.grounded) //air dive
        {
            Debug.Log(_rb.velocity.y);
            if (_rb.velocity.y < -6)
            {
                if (facingLeft)
                {
                    _rb.velocity = new Vector2(-1 * divespeed, _rb.velocity.y);
                }
                else
                {
                    _rb.velocity = new Vector2(divespeed, _rb.velocity.y);
                }
            }
            else
            {
                float ybump = Mathf.Clamp( _rb.velocity.y * 10f , diveYbump, _rb.velocity.y + diveYbump);
                if (facingLeft)
                {
                    _rb.velocity = new Vector2(-1 * divespeed, ybump);
                }
                else
                {
                    _rb.velocity = new Vector2(divespeed, ybump);
                }
            }
        }
        else //ground dive
        {
            if (facingLeft)
            {
                _rb.velocity = new Vector2(_rb.velocity.x - 3, diveSpringHeight);
            }
            else
            {
                _rb.velocity = new Vector2(_rb.velocity.x + 3, diveSpringHeight);
            }
        }
        diveActRec = false;
    }
    private void DiveSpringBoost()
    {
        if (facingLeft)
        {
            int direction = -1;
            _rb.velocity = new Vector2(direction * (Mathf.Abs(storedSpeed) + diveSpringLength), diveSpringHeight);
        }
        else
        {
            _rb.velocity = new Vector2(storedSpeed + diveSpringLength, diveSpringHeight);
        }
        flipActRec = false;
        diveActRec = false;
    }

    private bool FloorDetect()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, floorLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int WallDirectionDetect()
    {
        RaycastHit2D leftwallcast = Physics2D.BoxCast(transform.position, wallBoxSize, 0, -transform.right, leftCastDistance, wallLayer);
        RaycastHit2D rightwallcast = Physics2D.BoxCast(transform.position, wallBoxSize, 0, transform.right, rightCastDistance, wallLayer);
        if (DetectWalls)
        {
            if (leftwallcast && rightwallcast)
            {
                return 2;
            }
            else if (leftwallcast)
            {
                return -1;
            }
            else if (rightwallcast)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 3;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * leftCastDistance, wallBoxSize);
        Gizmos.DrawWireCube(transform.position + transform.right * rightCastDistance, wallBoxSize);
    }
}
