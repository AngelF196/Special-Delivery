using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void SaveButtonPressed()
    {
        FileSaveSystem.SaveFileData(Locator.Instance.Player);
    }

    public void LoadButtonPressed()
    {
        FileSaveData fileData = FileSaveSystem.LoadFileData();
        // Call load data functions upon each applicable object (Player, managers, etc.)
        
    }
}
