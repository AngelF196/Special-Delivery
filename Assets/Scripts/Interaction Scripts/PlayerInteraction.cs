using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private StarterNPC currentStarterNPC;

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentStarterNPC != null)
        {
            currentStarterNPC.StartRace();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StarterNPC"))
        {
            currentStarterNPC = other.GetComponent<StarterNPC>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StarterNPC"))
        {
            currentStarterNPC = null;
        }
    }
}