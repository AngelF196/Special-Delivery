using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public TMP_Text dialogueText; 
    public DialogueSO dialogue;   

    void Start()
    {
        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogue != null)
        {
            ShowDialogue();
        }
    }

    void ShowDialogue()
    {
        if (!string.IsNullOrEmpty(dialogue.dialogueText)) // Ensure there is dialogue text
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = dialogue.dialogueText;  
        }
    }

    void EndDialogue()
    {
        dialogueText.gameObject.SetActive(false); 
    }
}
