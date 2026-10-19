using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _fileSelectMenu;
    [SerializeField] private GameManager _gameManager;

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

    public void GoToSelectedFile(string scene_name)
    {
        if (scene_name != "ElephantMemory")
            _gameManager.LoadScene(scene_name);
        else
        {
            string savePath = "C:/SaveFileTests/file1.sdf";
            if (File.Exists(savePath) )
                _gameManager.LoadScene(scene_name);
            else
                Debug.LogError("A save file doesn't exist at the following path: " + savePath);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
