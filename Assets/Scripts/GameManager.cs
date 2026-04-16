using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private GameObject _player;
    private PlayerMove _pm;
    private Rigidbody2D _playerRb;

    private bool _questIsActive = false;
    private TextMeshProUGUI _questTimer;
    private int _minutes = 0;
    private float _seconds = 0f;

    void Start()
    {
        _pm = _player.GetComponent<PlayerMove>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _questTimer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_dialogueController.conversationIsActive)
        {
            _pm.enabled = false;
        }
        else
        {
            _pm.enabled = true;
        }

        if (_questIsActive)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        _seconds += Time.deltaTime;
        _seconds = Mathf.Round(_seconds * 1000) / 1000.0f;
        
        string secondsText = _seconds.ToString();
        if (_seconds < 10.0f)
            secondsText = "0" + secondsText;
        
        // Milliseconds formatting
        if ( Mathf.Round(_seconds*1000) % 100 == 0)
            secondsText = secondsText + "00";
        else if ( Mathf.Round(_seconds*1000) % 10 == 0)
            secondsText = secondsText + "0";
        
        if (_seconds >= 60.0f)
        {
            _minutes++;
            _seconds = 0f;
        }
        
        string time = _minutes + ":" + secondsText;
        _questTimer.text = time;
    }

    public void EnteredConversation()
    {
        _pm.enabled = false;
    }

    public void ExitedConversation()
    {
        _pm.enabled = true;
    }

    // Event method added to the gamePausedEvent event
    public void GamePaused()
    {
        Debug.Log("game paused");
    }

    // Event method added to the gameResumedEvent event
    public void GameResumed()
    {
        Debug.Log("game resumed");
    }

    // Event method for the DialogueController's activateQuest event
    public void QuestStarted(Quest questToActivate)
    {
        Debug.Log("A quest named \"" + questToActivate.questName + "\" has started! Starting timer...");
        _minutes = 0;
        _seconds = 0f;
        _questTimer.enabled = true;
        _questIsActive = true;
        
        GameObject endpoint = new GameObject("EndPoint", typeof(BoxCollider2D), typeof(EndPoint));
        endpoint.GetComponent<BoxCollider2D>().isTrigger = true;
        endpoint.transform.position = questToActivate.endpointCoordinates;
    }

    // Event method for the EndPoint's arrivedAtEnd event.
    public void QuestEnded()
    {
        Debug.Log("This quest has ended. Stopping timer...");
        _questIsActive = false;
    }
}
