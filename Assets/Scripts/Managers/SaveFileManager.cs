using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    public static SaveFileManager Instance {get; private set;}
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);  // This must persist throughout the entire game
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void SaveButtonPressed()
    {
        FileSaveSystem.SaveFileData(Locator.Instance.PlayerMove, Locator.Instance.LevelManager);
        print("File successfully saved");
    }

    public void LoadButtonPressed()
    {
        FileSaveData fileData = FileSaveSystem.LoadFileData();
        // Call load data functions upon each applicable object (Player, managers, etc.)
        Locator.Instance.LevelManager.LoadCollectibles(fileData);
        Locator.Instance.LevelManager.LoadConversationData(fileData);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "ElephantMemory")
        {
            print("Savable scene loaded");
            LoadButtonPressed();
        }
    }
}
