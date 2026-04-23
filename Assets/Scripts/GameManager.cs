using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private GameObject _player;
    [SerializeField] private BGM _musicManager;
    private PlayerMove _pm;
    private Rigidbody2D _playerRb;

    void Start()
    {
        _pm = _player.GetComponent<PlayerMove>();
        _playerRb = _player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_dialogueController.conversationIsActive)
        {
            _pm.enabled = false;
        }
        else
        {
            _pm.enabled = true;
        }
    }

    // Event method added to the gamePausedEvent event
    public void GamePaused()
    {
        _musicManager.PauseTransition();
        Debug.Log("game paused");
    }

    // Event method added to the gameResumedEvent event
    public void GameResumed()
    {
        _musicManager.GameTransition();
        Debug.Log("game resumed");
    }
}
