using UnityEngine;
using TMPro;

public class EndNPC : ParentNPC
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
        UpdateTimerUI(); // Update ui with timer
    }


    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString() + "s"; // Display as whole seconds
        }
    }

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
                    isTimerRunning = false; // Stop the timer
                    Debug.Log("Timer stopped. Total time: " + timer + " seconds");
                    timer = 0f; 
                }
                break;
            case NPCState.Active:
                break;
        }
    }
}
