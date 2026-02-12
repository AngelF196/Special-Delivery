using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum state
    {
        grounded, jumping, midair, diving, divelanding, walled, boosting
    }

    [Header("Ground Variables")]
    [SerializeField] private float accelRate;
    [SerializeField] private float decelRate;
    [SerializeField] private float maxSpeed;
    private float acceleration;

    [Header("Air Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float handspringMult;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float jumpcut;
    [SerializeField] private float airspeedmod;

    [Header("Flip/Dive Variables")]
    [SerializeField] private float flipJumpForce;
    [SerializeField] private float diveBoost;
    [SerializeField] private bool hasFlipped; //hide
    [SerializeField] private float diveSpringHeight;
    [SerializeField] private float diveSpringLength;
    [SerializeField] private float divespeedmod;
    [SerializeField] private float diveYbump;
    [SerializeField] private float diveLandMaxTime;
    private float diveLandTimer;

    [Header("Wall Variables")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpXForce;
    [SerializeField] private float wallJumpMult;
    [SerializeField] private float wallDashForce;
    [SerializeField] private bool hasWallDashed;

    // References
    private Rigidbody2D _rb;
    PlayerBoost _boost;
    PlayerEnvironment _collision;
    PlayerInput _inputs;
    PlayerAnimation _animation;

    //misc shit
    private state playerState;
    private state prevState;
    private float storedSpeed;

    //Shit for other scripts
    public state currentState => playerState;
    public float baseMaxSpeed => maxSpeed;
    public bool isFacingLeft => facingLeft;

    private bool facingLeft; //Don't remove

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boost = GetComponent<PlayerBoost>();
        _inputs = GetComponent<PlayerInput>();
        _collision = GetComponent<PlayerEnvironment>();
        _animation = GetComponentInChildren<PlayerAnimation>();

        hasFlipped = false;
        diveLandTimer = diveLandMaxTime;
    }

    void Update()
    {
        DirectionFacing(false);
    }

    private void FixedUpdate()
    {
        Action();

        if (playerState == state.divelanding)
        {
            diveLandTimer -= Time.fixedDeltaTime;

            if (diveLandTimer <= 0)
            {
                UpdateState(state.grounded);
                diveLandTimer = diveLandMaxTime;
            }

        }
    }

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                MovementCalc();

                if (_inputs.saysJump)
                {
                    UpdateState(state.jumping);
                }
                if (_collision.FloorDetect() == false) //Covers if going straight from ground to airborne
                {
                    if (_rb.velocity.y > 0)
                    {
                        UpdateState(state.jumping, false);
                    }
                    else
                    {
                        UpdateState(state.midair);
                    }
                }
                if (_inputs.saysDive || _inputs.saysFlip)
                {
                    UpdateState(state.diving);
                    _inputs.Consume(PlayerInput.Action.dive);
                    _inputs.Consume(PlayerInput.Action.flip);
                }
                break;
            case state.jumping:
                MovementCalc();

                if (_inputs.jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpcut * _rb.velocity.y);
                }
                if (_inputs.saysFlip && !hasFlipped)
                {
                    AirFlip();
                }
                if (_inputs.saysDive)
                {
                    UpdateState(state.diving);
                }
                if (_rb.velocity.y <= 0f)
                {
                    UpdateState(state.midair);
                }
                if (_collision.FloorDetect() && _rb.velocity.y <= 0f)
                {
                    UpdateState(state.grounded);
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
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
                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_inputs.saysFlip && !hasFlipped)
                {
                    AirFlip();
                }
                if (_inputs.saysDive)
                {
                    UpdateState(state.diving);
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.diving:
                MovementCalc();

                if (_collision.FloorDetect() && _rb.velocity.y <= 0)
                {
                    UpdateState(state.divelanding);
                }
                //wall bonk
                break;
            case state.divelanding:
                if (_inputs.saysFlip || _inputs.saysDive)
                {
                    UpdateState(state.boosting);
                }
                if (_inputs.saysJump)
                {
                    UpdateState(state.jumping);
                }

                //family guy death pose
                break;
            case state.boosting:
                MovementCalc();

                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_inputs.saysFlip)
                {
                    AirFlip();
                }
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
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
                if (_collision.WallDirectionDetect() == 0)
                {
                    UpdateState(state.midair);
                }
                if (_collision.FloorDetect())
                {
                    UpdateState(state.grounded);
                }
                if (_inputs.saysJump)
                {
                    UpdateState(state.jumping);
                }
                break;
        }
    }
    
    private void UpdateState(state newstate, bool doAction = true)
    {
        Debug.Log(newstate.ToString() + " state");
        prevState = playerState;
        playerState = newstate;
        _animation.UpdateAnimationState(newstate, prevState);
        if (doAction)
        {
            StateAction(newstate);
        }
    }
    private void StateAction(state newstate)
    {
        switch (newstate)
        {
            case state.jumping:
                if(prevState == state.divelanding)
                {
                    if (MathF.Sign(_inputs.RawDirections.x) != MathF.Sign(storedSpeed)) 
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
                    _rb.velocity = new Vector2(-_collision.WallDirectionDetect() * wallJumpXForce * Time.fixedDeltaTime, wallJumpMult * jumpForce * Time.fixedDeltaTime); //wall jump
                }
                else if (prevState == state.midair)
                {
                    _collision.DetectWalls = true;
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce * Time.fixedDeltaTime); //jump
                }

                _inputs.Consume(PlayerInput.Action.jump);
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
                //_rb.velocity = new Vector2(0f, _rb.velocity.y);
                break;
            case state.diving:
                Dive();
                break;
            case state.boosting:
                DiveSpringBoost();
                break;
            case state.walled:
                hasFlipped = false;
                if (prevState != state.walled)
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                break;
            case state.midair:
                _collision.DetectWalls = true;
                break;
        }
        if (prevState == state.walled)
        {
            _collision.DetectWalls = false;
            _boost.ResetWallTimer();
        }
        
    }
    private void DirectionFacing(bool flipped)
    {
        if (flipped == false && !hasFlipped)
        {
            if (_rb.velocity.x < 0)
            {
                facingLeft = true;
            }
            else if (_rb.velocity.x > 0)
            {
                facingLeft = false;
            }
        }
        else if (flipped && hasFlipped)
        {
            if (_inputs.RawDirections.x == -1)
            {
                facingLeft = true;
            }
            else if (_inputs.RawDirections.x == 1)
            {
                facingLeft = false;
            }
        }
    }
    private void MovementCalc()
    {
        float targetSpeed = _inputs.RawDirections.x * _boost.CurrentMaxSpeed(); //reflects left/right input
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
        if ((_inputs.saysFlip || _inputs.saysDive) && !hasWallDashed) //wall dash
        {
            _rb.velocity = new Vector2(0, wallDashForce);
            hasWallDashed = true;
            _animation.WallClimbAnimation();
            _inputs.Consume(PlayerInput.Action.flip);
            _inputs.Consume(PlayerInput.Action.dive);
        }
        else if (_inputs.saysFlip || _inputs.saysDive)
        {
            _inputs.Consume(PlayerInput.Action.flip);
            _inputs.Consume(PlayerInput.Action.dive);
        }
        if (_collision.WallDirectionDetect() == _inputs.RawDirections.x && _rb.velocity.y < 0) //wall slide
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
        _animation.FlipAnimation();
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * flipJumpForce, ForceMode2D.Impulse);

        hasFlipped = true;
        _inputs.Consume(PlayerInput.Action.flip);
        DirectionFacing(true);
    }
    private void Dive()
    {
        float forwardSpeed = Mathf.Abs(_rb.velocity.x);
        float divespeed = Mathf.Clamp(forwardSpeed, 5f, _boost.CurrentMaxSpeed());
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
        _inputs.Consume(PlayerInput.Action.dive);
    }
    private void DiveSpringBoost()
    {
        _boost.IncrementStage();
        if (facingLeft)
        {
            int direction = -1;
            _rb.velocity = new Vector2(direction * _boost.CurrentMaxSpeed(), diveSpringHeight);
        }
        else
        {
            _rb.velocity = new Vector2(_boost.CurrentMaxSpeed(), diveSpringHeight);
        }
        _inputs.Consume(PlayerInput.Action.flip);
        _inputs.Consume(PlayerInput.Action.dive);
    }


}
