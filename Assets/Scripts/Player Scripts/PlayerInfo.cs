using Cinemachine;
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

    public static PlayerInfo Instance { get; private set; }
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void Start()
    {
        _respawnPoint = transform.position;
        _camera = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();

        if (_camera is not null) _camera.Follow = Instance.gameObject.transform;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _camera = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        if (_camera is not null) _camera.Follow = Instance.gameObject.transform;
    }

    public void UpdateRespawn(Vector3 newPoint)
    {
        _respawnPoint = newPoint;
    }
}
