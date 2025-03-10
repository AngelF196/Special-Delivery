using UnityEngine;
using TMPro; 

public class StarterNPC : ParentNPC
{
    public float timer = 0f;
    private bool isTimerRunning = false;
    public TMP_Text timerText;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        HandleTimer();
        UpdateTimerUI(); // Update UI with timer value
    }

    void HandleTimer()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString() + "s";
        }
    }// displays as seconds 

    protected override void HandleState()
    {
        switch (currentState)
        {
            case NPCState.Idle:
                base.HandleState();
                break;
            case NPCState.Waving:
                base.HandleState();
                break;
            case NPCState.Interact:
                base.HandleState();
                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    isTimerRunning = true; // Start the timer when interaction happens
                    Debug.Log("Timer started");
                }
                break;
            case NPCState.Active:
                break;
        }
    }
}
