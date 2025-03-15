using TMPro;
using UnityEngine;

public class StarterNPC : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        
        TimerManager.Instance.timerText = timerText;
    }

    public void StartRace()
    {
        TimerManager.Instance.StartTimer();
        timerText.gameObject.SetActive(true);
    }
}
