using TMPro;
using UnityEngine;

public class StarterNPC : ParentNPC  
{
    [SerializeField] private TMP_Text timerText;

    protected override void Start()
    {
        base.Start();  

        TimerManager.Instance.timerText = timerText;
    }

    public void StartRace()
    {
        TimerManager.Instance.StartTimer();
        timerText.gameObject.SetActive(true);
    }

    protected override void HandleState()
    {
        base.HandleState(); 
      
    }
}
