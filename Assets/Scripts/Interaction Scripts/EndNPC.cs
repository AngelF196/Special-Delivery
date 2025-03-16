using UnityEngine;

public class EndNPC : ParentNPC  
{
    protected override void Start()
    {
        base.Start();  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimerManager.Instance.StopTimer();
            
        }
    }

    protected override void HandleState()
    {
        base.HandleState();  
    }
}
