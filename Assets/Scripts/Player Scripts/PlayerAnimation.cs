using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    GameObject _playerObject;
    PlayerMove _baseMovement;
    PlayerBoost _boost;

    private bool grounded;
    private bool walled;


    void Start()
    {
        PlayerMove Player = GameObject.Find("player").GetComponent<PlayerMove>();

        _boost = GetComponent<PlayerBoost>();
        _baseMovement = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            //idle
            //walking
            //running
            //boost stage run cycles
        }
        if (_boost.boostStage > 0)
        {
            //after images
        }
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
                grounded = true;
                break;
            case (PlayerMove.state.jumping):
                
                break;
            case (PlayerMove.state.midair):
                break;
            case (PlayerMove.state.diving):
                break;
            case (PlayerMove.state.divelanding):
                break;
            case (PlayerMove.state.walled):
                break;
            case (PlayerMove.state.boosting):
                break;
        }
    }

}
