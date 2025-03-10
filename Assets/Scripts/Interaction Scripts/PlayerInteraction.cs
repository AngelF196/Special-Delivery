using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private NPCInteraction currentNPC;

    void Update()
    {
        if (currentNPC != null && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Interact button pressed");
            currentNPC.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered with: " + other.name);
        if (other.CompareTag("NPC"))
        {
            Debug.Log("NPC detected: " + other.name);
            currentNPC = other.GetComponent<NPCInteraction>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log("Player exited NPC trigger zone");
            currentNPC = null;
        }
    }
}