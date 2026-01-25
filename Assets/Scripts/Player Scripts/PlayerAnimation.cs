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
    private bool grounded;
    private bool walled;


    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _boost = GetComponentInParent<PlayerBoost>();
        _baseMovement = GetComponentInParent<PlayerMove>();
        _animator = GetComponentInParent<Animator>();
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

            if (_rb.velocity.x != 0f)
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
                    walled = false;
                    grounded = true;
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Wall_Slide", false);
                    break;
                case (PlayerMove.state.jumping):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Jumping", true);
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Wall_Slide", false);
                    break;
                case (PlayerMove.state.midair):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Falling", true);
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("Wall_Slide", false);
                    break;
                case (PlayerMove.state.diving):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Diving", true);
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Wall_Slide", false);
                    break;
                case (PlayerMove.state.divelanding):
                    walled = false;
                    grounded = false;
                    _animator.SetBool("Diving", false);
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Wall_Slide", false);
                    break;
                case (PlayerMove.state.walled):
                    grounded = false;
                    _animator.SetBool("Grounded", false);
                    _animator.SetBool("Wall_Slide", true);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Jumping", false);
                    break;
                case (PlayerMove.state.boosting):
                    _animator.SetBool("Running", false);
                    break;
            }
        }

    }