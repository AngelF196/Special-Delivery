using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class NPCInteraction : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private bool hasInteracted = false;
    private bool dialogueActive = false;


    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    // Press E for INteraction, opens closes
    public void Interact()
    {
        if (!dialogueActive)
        {
            OpenDialogue();
        }
        else
        {
            CloseDialogue();
        }
    }

    void OpenDialogue()
    {
        dialogueActive = true;
        hasInteracted = true; 
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Dialogue Panel is not assigned.");
        }
    }

    void CloseDialogue()
    {
        dialogueActive = false;
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}
