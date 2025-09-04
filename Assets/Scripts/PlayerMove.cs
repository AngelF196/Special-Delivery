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

    // Player Variables
    [SerializeField] private float accelRate;
    [SerializeField] private float decelRate;
    private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;

    // Player Input
    private Vector2 playerDirections;
    private Vector2 rawPlayerDirections;
    [SerializeField] private state playerState;
    [SerializeField] private bool jumpRec;
    private bool jumpCutRec;
    private bool diveActRec;

    // References
    private Rigidbody2D _rb;
    private Collider2D _collider;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
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

        jumpRec = Input.GetKey(KeyCode.Space);
        jumpCutRec = Input.GetKey(KeyCode.Space) == false && playerState == state.jumping;
        diveActRec = Input.GetKey(KeyCode.LeftShift);
        
    }

    private void CollisionDetect()
    {

    }

    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                //movement
                float targetSpeed = rawPlayerDirections.x * maxSpeed; //reflects left/right input
                float currentSpeed = _rb.velocity.x;
                acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;
                float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
                _rb.velocity = new Vector2(newSpeed, _rb.velocity.y);
                
                //jump and jump state switch
                if (jumpRec)
                {
                    Debug.Log("changed state");
                    UpdateState(state.jumping);
                }

                //dive and dive state switch

                break;
            case state.jumping:
                //light restricted movement
                //dive and dive state switch
                //jump cut and midair state switch
                //midair state switch
                //walled state switch
                break;
            case state.midair:
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

    private void UpdateState(state newstate)
    {
        playerState = newstate;
    }
}
