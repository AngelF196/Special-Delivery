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

    //Player Variables
    [SerializeField] private float accelSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed;

    // Player Input
    private Vector2 playerDirections;
    [SerializeField] private state playerState;
    private bool jumpRec;
    private bool jumpCutRec;
    private bool diveActRec;

    void Start()
    {

    }

    // Update is called once per frame
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

        jumpRec = Input.GetKeyDown("space");
        jumpCutRec = Input.GetKeyUp("space") && playerState == state.grounded;
        diveActRec = Input.GetKeyDown("shift");
        
    }

    private void CollisionDetect()
    {

    }
    private void Action()
    {
        switch (playerState)
        {
            case state.grounded:
                //movement and skidding
                //jump and jump state switch
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
}
