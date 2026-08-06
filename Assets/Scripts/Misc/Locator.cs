using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public static Locator Instance {get; private set;}
    public PlayerMove Player {get; private set;}
    public GameManager GameManager {get; private set;}
    public SaveFileManager SaveFileManager {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        GameObject[] gameObjectManagers = GameObject.FindGameObjectsWithTag("Manager");
        GameManager = GetSpecifiedManagerGO("Game", gameObjectManagers).GetComponent<GameManager>();
        SaveFileManager = GetSpecifiedManagerGO("SaveFile", gameObjectManagers).GetComponent<SaveFileManager>();
    }

    private GameObject GetSpecifiedManagerGO(string managerName, GameObject[] managers)
    {
        foreach (GameObject manager in managers)
        {
            if (manager.name.Contains(managerName)) return manager;
        }
        return null;
    }
}
