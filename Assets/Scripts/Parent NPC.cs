using TMPro;
using UnityEngine;

public class ParentNPC : MonoBehaviour
{
    public enum NPCState { Idle, Waving, Interact, Active }

    public NPCState currentState;
    public Sprite idleSprite;
    public Sprite wavingSprite;
    public Sprite interactSprite;
    public SpriteRenderer spriteRenderer;
    public Transform player;

    public float timer = 0f;
    private bool isTimerRunning = false;
    public TMP_Text timerText;

    public float idleRange = 5f;
    public float curiousRange = 3f;
    public float provokedRange = 1f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        UpdateState();
        HandleState();
        HandleTimer();
        UpdateTimerUI();
    }

    // Handles the timer logic
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
    }

    // Update state based on player distance
    void UpdateState()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= provokedRange)
        {
            currentState = NPCState.Interact;
        }
        else if (distance <= curiousRange)
        {
            currentState = NPCState.Waving;
        }
        else
        {
            currentState = NPCState.Idle;
        }
    }

    // Handles state-specific actions
    protected virtual void HandleState()
    {
        switch (currentState)
        {
            case NPCState.Idle:
                spriteRenderer.sprite = idleSprite;
                break;
            case NPCState.Waving:
                spriteRenderer.sprite = wavingSprite;
                break;
            case NPCState.Interact:
                spriteRenderer.sprite = interactSprite;
                break;
            case NPCState.Active:
                break;
        }
    }

    // Start the timer for interaction
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // Stop the timer for interaction
    public void StopTimer()
    {
        isTimerRunning = false;
        timer = 0f;
    }
}
