using TMPro;
using UnityEngine;

public class StarterNPC : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        // Link to TimerManager's text display
        TimerManager.Instance.timerText = timerText;
    }

    public void StartRace()
    {
        TimerManager.Instance.StartTimer();
        timerText.gameObject.SetActive(true);
    }
}
