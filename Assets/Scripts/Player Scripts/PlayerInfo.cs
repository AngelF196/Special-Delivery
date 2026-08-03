using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    private Vector3 _respawnPoint = Vector3.zero;
    public Vector3 respawnPoint => _respawnPoint;

    [SerializeField] private string[] _nonLevels;
    public string lastLevelScene = "";
    void Start()
    {
        DontDestroyOnLoad(this);
        _respawnPoint = transform.position;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_nonLevels.Contains(scene.name)) lastLevelScene = scene.name;
    }

    public void UpdateRespawn(Vector3 newPoint)
    {
        _respawnPoint = newPoint;
    }
}
