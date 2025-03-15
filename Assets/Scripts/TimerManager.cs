using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    public float currentTime;
    public TMP_Text timerText;
    private bool isTimerActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Update()
    {
        if (isTimerActive)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        isTimerActive = true;
        currentTime = 0f;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = $"Time: {currentTime.ToString("F2")}s";
    }
}
