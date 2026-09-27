using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LoadZone : MonoBehaviour
{
    [SerializeField] private bool _automatic = true;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private string _sceneNameToLoad;

    private bool _waitingForInput = false;
    private PlayerInput input;
    private PlayerInfo _playerInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            input = collision.gameObject.GetComponent<PlayerInput>();
            _playerInfo = collision.gameObject.GetComponent<PlayerInfo>();
            if (_automatic)
            {
                LoadScene();
            }
            else
            {
                _waitingForInput = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            input = null;
            _waitingForInput = false;
            _playerInfo = null;
        }
    }

    private void Update()
    {
        if (_waitingForInput && input.RawDirections == _direction)
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        _playerInfo.lastLevelScene = SceneManager.GetActiveScene().name.ToString();
        SceneManager.LoadScene($"{_sceneNameToLoad}");
    }
}