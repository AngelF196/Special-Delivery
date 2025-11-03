using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainPauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _quitButton;
    private bool _gamePaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gamePaused = !_gamePaused;
            _pauseMenu.SetActive(_gamePaused);
            _mainPauseMenu.SetActive(_gamePaused);
            _optionsMenu.SetActive(false);
            Time.timeScale = 0.0f;

            if (!_gamePaused)
            {
                Time.timeScale = 1.0f;
            }
        }
    }

    public void ResumeButton()
    {
        _gamePaused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OptionsButton()
    {
        _mainPauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }
    
    public void BackButton()
    {
        _mainPauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }
}
