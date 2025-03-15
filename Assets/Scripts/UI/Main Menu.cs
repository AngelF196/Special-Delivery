using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private string MAINSCENE;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(MAINSCENE);
    }
}