using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private int maxBoostStage = 4;
    [SerializeField] private float[] stageMaxSpeed;
    [SerializeField] private float[] stageMinSpeed;
    [SerializeField] private float[] stageDuration;

    [Header("Wall Buffer")]
    [SerializeField] private float wallBufferMaxTime = 0.15f;

    //shit
    [SerializeField] private int currentStage;
    private float stageTimer;
    private float wallBufferTimer;
    private bool boosting;
    private bool wallTimerStarted;
    public int boostStage => currentStage;
    public bool isBoosting => boosting;

    //References
    PlayerMove _baseMovement;
    Rigidbody2D _rb;

    private void Start()
    {
        _baseMovement = GetComponent<PlayerMove>();
        _rb = GetComponent<Rigidbody2D>();

        currentStage = 0;
        stageTimer = 0f;
        wallBufferTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (boosting)
        {
            HandleTimers();
            EnforceSpeed();
        }
    }


    public void IncrementStage()
    {
        if (currentStage >= maxBoostStage)
        {
            return;
        }
        else
        {
            if (!boosting)
            {
                boosting = true;
            }
            currentStage++;
            stageTimer = stageDuration[currentStage];
        }
    }
    private void StopBoosting()
    {
        currentStage = 0;
        boosting = false;
        stageTimer = 0f;
        ResetWallTimer();
    }

    private void HandleTimers()
    {
        stageTimer -= Time.fixedDeltaTime;

        if(_baseMovement.currentState == PlayerMove.state.walled && !wallTimerStarted)
        {
            wallTimerStarted = true;
            wallBufferTimer = wallBufferMaxTime;
        }
        else if (_baseMovement.currentState == PlayerMove.state.walled && wallTimerStarted) 
        {
            wallBufferTimer -= Time.fixedDeltaTime;
        }

        if (stageTimer < 0f) 
        { 
            StopBoosting();
        }
    }


    private void EnforceSpeed()
    {
        float speed = Mathf.Abs(_rb.velocity.x);

        if (_baseMovement.currentState == PlayerMove.state.walled && wallBufferTimer > 0)
        {
            return;
        }
        if (speed < stageMinSpeed[currentStage])
        {
            currentStage--;
            // StopBoost();

            if (currentStage <= 0)
                StopBoosting();
            else
                stageTimer = stageDuration[currentStage];
        }
    }
    public float CurrentMaxSpeed()
    {
        if(!boosting) {  return _baseMovement.baseMaxSpeed; }
        else { return stageMaxSpeed[currentStage]; }
    }

    public void ResetWallTimer()
    {
        wallBufferTimer = 0f;
        wallTimerStarted = false;
    }
}
