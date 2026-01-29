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

    private Queue<string> _paragraphs = new Queue<string>();
    private bool _conversationEnded = false;
    private string _line;

    private Coroutine typeDialogueCoroutine;
    private bool _isTyping;

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
            // Assigns the line that is removed from the queue, and then type it out
            _line = _paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeOutLine(_line));
        }
        else
            FinishLineEarly();

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

        _characterName.text = convo.speakerName;

        for (int i = 0; i < convo.dialogueLines.Length; i++)
        {
            // Adds a line of dialogue to the queue
            _paragraphs.Enqueue(convo.dialogueLines[i]);
        }
    }

    private void EndConversation()
    {
        _conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
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
