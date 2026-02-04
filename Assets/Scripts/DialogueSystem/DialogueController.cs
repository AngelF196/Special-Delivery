using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    [Header("Dialogue System References")]
    [SerializeField] private Image _leftCharacterPortrait;
    [SerializeField] private Image _rightCharacterPortrait;
    [SerializeField] private TextMeshProUGUI _characterName;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [Header("Dialogue System Controls")]
    [SerializeField] private int _charactersPerSecond = 40;
    [SerializeField, Tooltip("The combination of (preferably special) characters where, when detected, will stop for a moment and then yell out something. This delimiter must be present in at least one spot to indicate the start.\n\nWhile the ending delimiter (second one) is optional, if it's not present, it is assumed that it will be in the end of a dialogue line.")]
    private string _yellingDelimiter = "^^";
    [SerializeField, Tooltip("The amount of time the typing is paused after arriving at the delimiter. This pause takes effect each time the controller arrives at a dialogue delimiter.")]
    private float _delimiterPauseTime = 0.5f;

    [Header("Debug Stuff")]
    [SerializeField] private Color _talkingColor = Color.white;
    [SerializeField] private Color _listeningColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private Queue<Conversation> _conversationObjs = new Queue<Conversation>();
    private bool _conversationEnded = false;
    private string _name;
    private string _line;
    private int _index = 0;
    private bool _isInConvo = false;

    private Coroutine typeDialogueCoroutine;
    private bool _isTyping;

    // Public variables for use with other scripts
    public bool conversationIsActive => _isInConvo;

    public void DisplayNextLine(Conversation conversation)
    {
        // After intreracting with the NPC, check if there' nothing in its Queue
        // Because the queue is empty upon initialization, the conversation will start
        if (_conversationObjs.Count == 0)
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
            UpdateDialogueUI();
        else
            FinishLineEarly();

        // Make a check to ensure all lines of dialogue are removed from the queue
        if(_conversationObjs.Count == 0)
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
            _conversationObjs.Enqueue(convo);
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
        _index = 0;
    }

    private void UpdateDialogueUI()
    {
        Conversation.DialogueLine nextLine = _conversationObjs.Dequeue().dialogueLines[_index];
        _line = nextLine.dialogueText;
        _name = nextLine.speakerName;
        _index++;
        switch (nextLine.portraitSideToHighlight)
        {
            case Conversation.PortraitSide.LEFT_SIDE:
                _leftCharacterPortrait.color = _talkingColor;
                _rightCharacterPortrait.color = _listeningColor;
                break;
            case Conversation.PortraitSide.RIGHT_SIDE:
                _leftCharacterPortrait.color = _listeningColor;
                _rightCharacterPortrait.color = _talkingColor;
                break;
        }


        _characterName.text = _name;
        typeDialogueCoroutine = StartCoroutine(TypeOutLine(_line));
    }

    private IEnumerator TypeOutLine(string line)
    {
        _isTyping = true;

        int maxVisibleChars = 0;  // Index for individual characters in a dialogue line & updater on the showing of characters
        int delIndexLocation = line.IndexOf(_yellingDelimiter);

        _dialogueText.text = line;
        _dialogueText.maxVisibleCharacters = maxVisibleChars;
        
        Debug.Log("First location of delimiter: " + delIndexLocation);
        foreach(char c in _dialogueText.text.ToCharArray())
        {
            if (maxVisibleChars == delIndexLocation)
            {
                string subStr = _dialogueText.text.Substring(delIndexLocation + _yellingDelimiter.Length);
                _dialogueText.text = _dialogueText.text[0..delIndexLocation] + subStr;

                delIndexLocation = _dialogueText.text.IndexOf(_yellingDelimiter);
                Debug.Log("Next location of delimiter: " + delIndexLocation);
                yield return new WaitForSeconds(_delimiterPauseTime);
            }
            
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

        string lineWithoutDelimiters = _dialogueText.text;
        int delIndexLocation = lineWithoutDelimiters.IndexOf(_yellingDelimiter);
        while (delIndexLocation != -1)
        {
            string subStr = _dialogueText.text.Substring(delIndexLocation + _yellingDelimiter.Length);
            _dialogueText.text = _dialogueText.text[0..delIndexLocation] + subStr;

            delIndexLocation = _dialogueText.text.IndexOf(_yellingDelimiter);
            Debug.Log("Next location of delimiter: " + delIndexLocation);
        }

        _dialogueText.maxVisibleCharacters = _dialogueText.text.Length;
        _isTyping = false;
    }
}
