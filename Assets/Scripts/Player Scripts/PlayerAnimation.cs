using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMove _baseMovement;
    private PlayerBoost _boost;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private PlayerEnvironment _environment;
    private bool grounded;
    private bool walled;


    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _boost = GetComponentInParent<PlayerBoost>();
        _baseMovement = GetComponentInParent<PlayerMove>();
        _animator = GetComponentInParent<Animator>();
        _environment = GetComponentInParent<PlayerEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_baseMovement.isFacingLeft)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }

        if (grounded)
        {
            _animator.SetBool("Grounded", true);

            if (_baseMovement.localVelocity.x != 0f)
            {
                _animator.SetBool("Running", true);
            }
            else
            {
                _animator.SetBool("Running", false);
            }

            if (_boost.isBoosting)
            {
                _animator.SetBool("Boosting", true);
            }
            else
            {
                _animator.SetBool("Boosting", false);
            }
            //idle
            //walking
            //running
            //boost stage run cycles
        }
    }
    public void FlipAnimation()
    {
        _animator.SetTrigger("Flip");
    }

    public void WallClimbAnimation()
    {
        _animator.SetTrigger("Wall_Climb");
    }

    public void UpdateAnimationState(PlayerMove.state state, PlayerMove.state prevState)
    {
        if (state != PlayerMove.state.grounded)
        {
            grounded = false;
        }

        switch (state)
        {
            case (PlayerMove.state.grounded):
                transform.localPosition = new Vector3(0, 0.402f, 0);
                walled = false;
                grounded = true;
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Falling", false);
                _animator.SetBool("Wall_Slide", false);
                _animator.SetBool("Diving", false);
                _animator.SetBool("Bonk_Land", false);

                break;
            case (PlayerMove.state.jumping):
                transform.localPosition = new Vector3(0, 0.402f, 0);
                walled = false;
                grounded = false;
                _animator.SetBool("Jumping", true);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Falling", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Wall_Slide", false);
                break;
            case (PlayerMove.state.midair):
                transform.localPosition = new Vector3(0, 0.402f, 0);
                walled = false;
                grounded = false;
                _animator.SetBool("Falling", true);
                _animator.SetBool("Running", false);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Wall_Slide", false);
                break;
            case (PlayerMove.state.diving):
                transform.localPosition = new Vector3(0, 0.402f, 0);
                walled = false;
                grounded = false;
                _animator.SetBool("Diving", true);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Wall_Slide", false);
                _animator.SetBool("Falling", false);
                break;
            case (PlayerMove.state.divelanding):
                walled = false;
                grounded = false;
                transform.localPosition = new Vector3(0, -0.47f, 0);
                _animator.SetBool("Diving", false);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Wall_Slide", false);
                _animator.SetBool("Falling", false);
                break;
            case (PlayerMove.state.walled):
                transform.localPosition = new Vector3(0, 0.402f, 0);

                grounded = false;
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Falling", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Jumping", false);
                if (_environment.WallDirectionDetect() == -1 && _baseMovement.isFacingLeft)
                {
                    _animator.SetBool("Wall_Slide", true);
                }
                else if (_environment.WallDirectionDetect() == 1 && !_baseMovement.isFacingLeft)
                {
                    _animator.SetBool("Wall_Slide", true);
                }
            break;
            case (PlayerMove.state.boosting):
                transform.localPosition = new Vector3(0, 0.402f, 0);

                _animator.SetBool("Running", false);
                break;

            case (PlayerMove.state.bonked):
                transform.localPosition = new Vector3(0, 0.402f, 0);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Wall_Slide", false);
                _animator.SetBool("Falling", false);
                _animator.SetBool("Bonked", true);
                _animator.SetBool("Jumping", false);

                break;

            case (PlayerMove.state.bonklanding):
                _animator.SetBool("Bonked", false);
                _animator.SetBool("Diving", false);
                _animator.SetBool("Grounded", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Wall_Slide", false);
                _animator.SetBool("Falling", false);
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Bonk_Land", true);

                break;
        }
    }

}