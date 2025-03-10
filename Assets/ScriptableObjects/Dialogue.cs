using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string[] _lines;
    private int _currentLine;
    private bool _isDialogueActive;
    private TMP_Text _dialogueText;

    private NPCInteraction _npcInteraction;

    void Start()
    {
        _npcInteraction = GetComponent<NPCInteraction>();
        _dialogueText = _npcInteraction.dialogueText;  // Reference to TMP_Text from NPCInteraction
        _npcInteraction.dialoguePanel.SetActive(false); // Start with panel hidden
    }

    void Update()
    {
        // If dialogue is active, show the next line on key press (E)
        if (_isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue()
    {
        _npcInteraction.dialoguePanel.SetActive(true);  // Show the dialogue panel
        _currentLine = 0; // Start from the first line
        ShowDialogue();
        _isDialogueActive = true;  // Dialogue is now active
    }

    public void EndDialogue()
    {
        _npcInteraction.dialoguePanel.SetActive(false); // Hide the dialogue panel
        _isDialogueActive = false; // Dialogue is no longer active
    }

    private void AdvanceDialogue()
    {
        _currentLine++;

        if (_currentLine < _lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue(); // End dialogue when all lines have been shown
        }
    }

    private void ShowDialogue()
    {
        _dialogueText.text = _lines[_currentLine];  // Display current line
    }
}
