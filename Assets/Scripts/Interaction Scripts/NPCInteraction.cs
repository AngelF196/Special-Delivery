using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    private bool hasInteracted = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void Interact()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            StartMission();
            ShowDialogue();
        }
    }

    void StartMission()
    {
        Debug.Log("Mission Started");
    }

    void ShowDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "Here's your package. Your mission has begun!";
        }
    }
}
