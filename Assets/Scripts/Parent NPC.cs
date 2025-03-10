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
    }

    // Update state based on player distance
    void UpdateState()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= provokedRange)
        {
            currentState = NPCState.Interact; // Player is close enough for interaction
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
                // Active behavior can be implemented by child classes
                break;
        }
    }
}
