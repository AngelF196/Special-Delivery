using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _characterName;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private int _charactersPerSecond = 20;

    private Queue<string> _dialogueName = new Queue<string>();
    private Queue<string> _paragraphs = new Queue<string>();
    private bool _conversationEnded = false;
    private string _name;
    private string _line;
    private bool _isInConvo = false;

    private Coroutine typeDialogueCoroutine;
    private bool _isTyping;

    // Public variables for use with other scripts
    public bool conversationIsActive => _isInConvo;
    public bool finishedConvo => _conversationEnded;

    public void DisplayNextLine(Conversation conversation)
    {
        // After intreracting with the NPC, check if there' nothing in its Queue
        // Because the queue is empty upon initialization, the conversation will start
        if (_paragraphs.Count == 0)
        {
            if (!_conversationEnded)
            {
                StartConversation(conversation);
            }
            else if (_conversationEnded && !_isTyping)
            {
                EndConversation();
                return;
            }
        }

        if (!_isTyping)
        {
            _name = _dialogueName.Dequeue();
            // Assigns the line that is removed from the queue, and then type it out
            _line = _paragraphs.Dequeue();

            _characterName.text = _name;
            typeDialogueCoroutine = StartCoroutine(TypeOutLine(_line));
        }
        else
            FinishLineEarly();

        // Make a check to ensure all lines of dialogue are removed from the queue
        if(_paragraphs.Count == 0)
        {
            _conversationEnded = true;
        }
    }

    private void StartConversation(Conversation convo)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        for (int i = 0; i < convo.dialogueLines.Length; i++)
        {
            // Adds a line of dialogue to the queue
            _paragraphs.Enqueue(convo.dialogueLines[i].dialogueText);
            _dialogueName.Enqueue(convo.dialogueLines[i].speakerName);
        }
        _isInConvo = true;
    }

    private void EndConversation()
    {
        _conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        _isInConvo = false;
    }

    private IEnumerator TypeOutLine(string line)
    {
        _isTyping = true;
        int maxVisibleChars = 0;

        _dialogueText.text = line;
        _dialogueText.maxVisibleCharacters = maxVisibleChars;

        foreach(char c in _line.ToCharArray())
        {
            maxVisibleChars++;
            _dialogueText.maxVisibleCharacters = maxVisibleChars;
            yield return new WaitForSeconds(1.0f / _charactersPerSecond);
        }
        

        _isTyping = false;
    }

    private void FinishLineEarly()
    {
        // Stops the TypeOutLine coroutine and shows the full line early
        StopCoroutine(typeDialogueCoroutine);

        _dialogueText.maxVisibleCharacters = _line.ToCharArray().Length;
        _isTyping = false;
    }
}
