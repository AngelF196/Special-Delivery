using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool printStates;
    public enum state
    {
        grounded, jumping, midair, diving, divelanding, walled, boosting
    }
    private enum restriction
    {
        light, heavy, none, diveLanding
    }

    [Header("Ground Variables")]
    [SerializeField] private float accelRate;
    [SerializeField] private float decelRate;
    [SerializeField] private float diveLandDecel;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector2 platformVelocity = Vector2.zero;
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
    [SerializeField] private float groundDiveLength;
    [SerializeField] private float groundDiveHeight;
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
    public bool holdingTowardsWall = false;
    public float baseMaxSpeed => maxSpeed;
    public bool isFacingLeft => facingLeft;

    public Vector2 localVelocity;
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
        if (playerState != state.walled)
        {
            DirectionFacing(false);
        }
    }

    private void FixedUpdate()
    {
        localVelocity = _rb.velocity - platformVelocity;
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
                HorizontalMovement(restriction.none);

                if (_inputs.saysJump) UpdateState(state.jumping);
                
                if (_collision.FloorDetect() == false) //Covers if going straight from ground to airborne
                {
                    if (_rb.velocity.y > 0) UpdateState(state.jumping, false);
                    
                    else UpdateState(state.midair);
                }
                if (_inputs.saysDive) UpdateState(state.diving);
                
                break;
            case state.jumping:
                HorizontalMovement(restriction.light);

                if (_inputs.jumpCutRec)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpcut * _rb.velocity.y);
                }
                if (_inputs.saysFlip && !hasFlipped) AirFlip();
                
                if (_inputs.saysDive) UpdateState(state.diving);
                
                if (_rb.velocity.y <= 0f) UpdateState(state.midair);
                
                if (_collision.FloorDetect() && _rb.velocity.y <= 0f) UpdateState(state.grounded);
                
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3) UpdateState(state.walled);
               
                break;
            case state.midair:
                HorizontalMovement(restriction.light);

                if (_rb.velocity.y <= -maxFallSpeed) _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                
                if (_collision.FloorDetect()) UpdateState(state.grounded);
                
                if (_inputs.saysFlip && !hasFlipped) AirFlip();
                
                if (_inputs.saysDive) UpdateState(state.diving);
                
                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3)
                {
                    UpdateState(state.walled);
                }
                break;
            case state.diving:
                HorizontalMovement(restriction.heavy);

                if (_collision.FloorDetect() && _rb.velocity.y <= 0) UpdateState(state.divelanding);
                
                //wall bonk
                break;
            case state.divelanding:
                HorizontalMovement(restriction.diveLanding);

                if (_inputs.saysFlip || _inputs.saysDive) UpdateState(state.boosting);                
                if (_inputs.saysJump) UpdateState(state.jumping);
                
                //family guy death pose
                break;
            case state.boosting:
                HorizontalMovement(restriction.none);

                if (_collision.FloorDetect()) UpdateState(state.grounded);
                
                if (_inputs.saysFlip && !hasFlipped) AirFlip();

                if (_collision.WallDirectionDetect() != 0 && _collision.WallDirectionDetect() != 3) UpdateState(state.walled);

                break;
            case state.walled:
                WallMovement();

                if (_rb.velocity.y <= -maxFallSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
                }

                if (_collision.WallDirectionDetect() == 0) UpdateState(state.midair);
                if (_collision.FloorDetect()) UpdateState(state.grounded);
                if (_inputs.saysJump) UpdateState(state.jumping);               
                break;
        }
    }
    
    private void UpdateState(state newstate, bool doAction = true)
    {
        if (printStates) Debug.Log(newstate.ToString() + " state");
        prevState = playerState;
        playerState = newstate;
        _animation.UpdateAnimationState(newstate, prevState);
        if (doAction) StateAction(newstate);
    }
    private void StateAction(state newstate)
    {
        switch (newstate)
        {
            case state.jumping:
                Jumping();
                break;

            case state.grounded:
                hasFlipped = false;
                hasWallDashed = false;
                break;

            case state.divelanding:
                hasFlipped = false;
                hasWallDashed = false;
                diveLandTimer = diveLandMaxTime;
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
                if (prevState == state.diving)
                {
                    UpdateState(state.diving, false);
                }
                break;

            case state.midair:
                _collision.DetectWalls = true;
                break;
        }
        if (prevState == state.walled)
        {
            _collision.DetectWalls = false;
            holdingTowardsWall = false;
            _boost.ResetWallTimer();
            _boost.StartWallGracePeriod();
        }
        
    }
    private void DirectionFacing(bool flipped)
    {
        if (flipped == false && !hasFlipped)
        {
            if (localVelocity.x < 0) facingLeft = true;
            
            else if (localVelocity.x > 0) facingLeft = false;         
        }
        else if (flipped && hasFlipped)
        {
            if (_inputs.RawDirections.x == -1) facingLeft = true;
            
            else if (_inputs.RawDirections.x == 1) facingLeft = false;   
        }
    }

    private void HorizontalMovement(restriction restriction)
    {
        float targetSpeed = _inputs.RawDirections.x * _boost.CurrentMaxSpeed(); //reflects left/right input
        float currentSpeed = _rb.velocity.x - platformVelocity.x;
        acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
        float newSpeed = 0;
        switch (restriction)
        {
            case restriction.none:
                newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime); //unrestricted movement
                break;
            case restriction.light:
                newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * airspeedmod * Time.fixedDeltaTime); //light restricted movement
                break;
            case restriction.heavy:
                newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * divespeedmod * Time.fixedDeltaTime); //heavy restricted movement
                break;
            case restriction.diveLanding:
                newSpeed = Mathf.MoveTowards(currentSpeed, (currentSpeed / diveLandDecel), acceleration * Time.fixedDeltaTime);
                break;
        }
        newSpeed += platformVelocity.x;
        _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);
    }

    private void WallMovement()
    {
        if (_inputs.saysFlip || _inputs.saysDive) //wall dash
        {
            if (!hasWallDashed)
            {
                _rb.velocity = new Vector2(0, wallDashForce + (_boost.boostStage * 2));
                hasWallDashed = true;
                _animation.WallClimbAnimation();
            }
            _inputs.Consume(PlayerInput.Action.flip);
            _inputs.Consume(PlayerInput.Action.dive);
        }
        if (_collision.WallDirectionDetect() == _inputs.RawDirections.x && _rb.velocity.y < 0) //wall slide
        {
            _rb.velocity = new Vector2(0, -wallSlideSpeed);
            holdingTowardsWall = true;
        }
        else //move away from wall
        {
            holdingTowardsWall = false;
            HorizontalMovement(restriction.light);
        }
    }

    private void Jumping()
    {
        float platformCarryY = Mathf.Max(0f, platformVelocity.y); // No negative influence
        float jumpY = jumpForce;
        float jumpX = _rb.velocity.x;

        if (prevState == state.jumping || prevState == state.midair) return;
        switch (prevState)
        {
            case state.divelanding: // Hand Spring
                if (MathF.Sign(_inputs.RawDirections.x) != MathF.Sign(storedSpeed))
                {
                    jumpX = storedSpeed / 2f; // Holding against momentum 
                }
                else jumpX = storedSpeed;
                
                jumpY = handspringMult * jumpForce;
                jumpX += platformVelocity.x;
                jumpY += platformCarryY;
                break;

            case state.walled: // Walljump
                if (!_boost.isBoosting)
                {
                    jumpX = -_collision.WallDirectionDetect() * wallJumpXForce;
                }
                else jumpX = -_collision.WallDirectionDetect() * _boost.CurrentMaxSpeed(); // boost wall jump

                jumpY = wallJumpMult * jumpForce;
                // jumpX += platformVelocity.x;
                // jumpY += platformCarryY;
                break;

            case state.midair:
                _collision.DetectWalls = true;
                break;

            default:
                jumpY += platformCarryY;
                break;
        }
        _rb.velocity = new Vector2(jumpX, jumpY);
        _inputs.Consume(PlayerInput.Action.jump);
        hasWallDashed = false;
    }

    private void AirFlip()
    {
        if (playerState == state.grounded) return;
        _animation.FlipAnimation();
        if (_rb.velocity.y <= 5) {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.AddForce(Vector2.up * flipJumpForce, ForceMode2D.Impulse);
        }

        hasFlipped = true;
        _inputs.Consume(PlayerInput.Action.flip);
        DirectionFacing(true);
    }

    private void Dive()
    {
        float forwardSpeed = Mathf.Abs(_rb.velocity.x);
        float divespeed = Mathf.Clamp(forwardSpeed, 5f, _boost.CurrentMaxSpeed());
        if (prevState == state.midair || prevState == state.jumping) //air dive
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            float ybump = Mathf.Clamp( _rb.velocity.y + 10f , diveYbump, _rb.velocity.y + diveYbump);
            if (facingLeft) _rb.velocity = new Vector2(-divespeed, ybump);
          
            else _rb.velocity = new Vector2(divespeed, ybump);
        }
        else //ground dive
        {
            if (facingLeft) _rb.velocity = new Vector2(_rb.velocity.x - groundDiveLength, groundDiveHeight);
            else _rb.velocity = new Vector2(_rb.velocity.x + groundDiveLength, groundDiveHeight);
        }
        _inputs.Consume(PlayerInput.Action.dive);
    }

    private void DiveSpringBoost()
    {
        _boost.IncrementStage();
        int direction = 0;
        if (facingLeft) direction = -1;
        else direction = 1;

        _rb.velocity = new Vector2(direction * _boost.CurrentMaxSpeed(), diveSpringHeight);
        
        _inputs.Consume(PlayerInput.Action.flip);
        _inputs.Consume(PlayerInput.Action.dive);
    }

    public void SetRigidBodyVelocity(Vector2 targetVelocity)
    {
        _rb.velocity = targetVelocity;
    }
    public void SetPlatformVelocity(Vector2 velocity)
    {
        platformVelocity = velocity;
    }
}
