using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _fileSelectMenu;

    public void GoToOptionsMenu()
    {
        _mainMenu.SetActive(false);
        _fileSelectMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void GoToFileSelect()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _fileSelectMenu.SetActive(true);
    }

    public void GoBack()
    {
        _optionsMenu.SetActive(false);
        _fileSelectMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void GoToSelectedFile()
    {
        SceneManager.LoadScene("City Blockout");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
