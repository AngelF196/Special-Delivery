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

    public DialogueSO dialogue;
    public TMP_Text dialogueText;
    private bool isDialogueActive = false;

    public float interactionRange = 2f;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        // Check the distance between the player and NPC
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= interactionRange && !isDialogueActive)
        {
            currentState = NPCState.Interact;
        }
        else if (distanceToPlayer <= 10f)  // Distance for waving
        {
            currentState = NPCState.Waving;
        }
        else
        {
            currentState = NPCState.Idle;
        }

        // Update state and handle interaction input
        HandleState();

        // Interaction
        if (distanceToPlayer <= interactionRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
                StartDialogue();
        }
    }

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

    void StartDialogue()
    {
        if (dialogue == null || string.IsNullOrEmpty(dialogue.dialogueText))
            return;

        isDialogueActive = true;
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = dialogue.dialogueText; 
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueText.gameObject.SetActive(false);
    }
}
