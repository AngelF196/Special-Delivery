using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GMInstance {get; private set;}
    
    // [SerializeField] private DialogueController _dialogueController;
    // [SerializeField] private GameObject _player;
    // Fuck the above references, use the Locator instead
    [SerializeField] private BGM _musicManager;

    public enum SceneTransition
    {
        none, slide, circle  // I dunno, these are random names I just came up with
    }

    // Event
    public UnityEvent<SceneTransition, Vector2> sceneTransition;

    // Ensures that the game manager is a singleton
    void Awake()
    {
        if (GMInstance != null && GMInstance != this)
        {
            Destroy(this);
            return;
        }
        GMInstance = this;
    }

    void Start()
    {
        // I'll figure out something to fill in here, if at all possible
    }

    void Update()
    {
        // Same with down here, I'll think of something. Though if I find them unnecessary, both these empty methods shall be executed
        // (as in erased from this script)
    }

    public void LoadScene(string sceneName, SceneTransition transition = SceneTransition.none, Vector2 position = default)
    {
        if (transition != SceneTransition.none) {
            sceneTransition.Invoke(transition, position);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void EnteredConversation()
    {
        // _pm.enabled = false;
    }

    public void ExitedConversation()
    {
        // _pm.enabled = true;
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
