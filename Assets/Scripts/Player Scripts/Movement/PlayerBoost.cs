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
    [SerializeField] private float wallGraceMaxTime = 0.2f;

    //shit
    [SerializeField] private int currentStage;
    private float stageTimer;
    private float wallBufferTimer;
    private float wallGraceTimer;
    private bool boosting;
    private bool wallTimerStarted;
    public int boostStage => currentStage;
    public bool isBoosting => boosting;
    private List<int> wallReturns = new List<int> {-1, 1, 2};

    //References
    private PlayerMove _baseMovement;
    private Rigidbody2D _rb;
    private PlayerEnvironment _environment;
    private PlayerAfterImages _afterImages;
    private void Start()
    {
        _baseMovement = GetComponent<PlayerMove>();
        _rb = GetComponent<Rigidbody2D>();
        _environment = GetComponent<PlayerEnvironment>();
        _afterImages = GetComponentInChildren<PlayerAfterImages>();

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
            stageTimer = stageDuration[currentStage - 1];
            return;
        }
        else
        {
            if (!boosting)
            {
                boosting = true;
            }
            currentStage++;
            if (currentStage == maxBoostStage) _afterImages.StartAfterImages();
            Debug.Log($"Stage Incremented To: {currentStage}");
            stageTimer = stageDuration[currentStage - 1];
        }
    }
    private void StopBoosting()
    {
        currentStage = 0;
        Debug.Log($"Stage Reset To: {currentStage}");
        boosting = false;
        stageTimer = 0f;
        _afterImages.StopAfterImages();
        ResetWallTimer();
    }

    private void HandleTimers()
    {
        stageTimer -= Time.fixedDeltaTime;

        if (wallReturns.Contains(_environment.WallDirectionDetect()) && !wallTimerStarted)
        {
            wallTimerStarted = true;
            wallBufferTimer = wallBufferMaxTime;
        }
        else if (wallReturns.Contains(_environment.WallDirectionDetect()) && wallTimerStarted) 
        {
            wallBufferTimer -= Time.fixedDeltaTime;
        }

        if (wallGraceTimer > 0f)
        {
            wallGraceTimer -= Time.fixedDeltaTime;
        }

        if (stageTimer < 0f) 
        {
            Debug.Log("Timer ran out!");
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
        if (wallGraceTimer <= 0 && currentStage != 0 && speed < stageMinSpeed[currentStage - 1])
        {
            currentStage--;
            if (currentStage < maxBoostStage) _afterImages.StopAfterImages();
            Debug.Log($"Stage Decremented To: {currentStage}");

            // StopBoost();

            if (currentStage <= 0)
                StopBoosting();
            else
                stageTimer = stageDuration[currentStage - 1];
        }
    }
    public float CurrentMaxSpeed()
    {
        if(!boosting) {  return _baseMovement.baseMaxSpeed; }
        else { return stageMaxSpeed[currentStage - 1]; }
    }

    public void ResetWallTimer()
    {
        wallBufferTimer = 0f;
        wallTimerStarted = false;
    }

    public void StartWallGracePeriod()
    {
        Debug.Log("started wall grace period");
        wallGraceTimer = wallGraceMaxTime;
    }
}
