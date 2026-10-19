using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance {get; private set;} // Only one QuestManager should be active at a time

    private bool _questIsActive = false;
    private TextMeshProUGUI _questTimer;
    private int _minutes = 0;
    private float _seconds = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _questTimer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        _questTimer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        StartCoroutine("HideTimer");
    }

    IEnumerator HideTimer()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        _questTimer.enabled = false;
    }
}
