using UnityEngine;

public class EndNPC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimerManager.Instance.StopTimer();
           
        }
    }
}
