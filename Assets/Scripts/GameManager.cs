using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private PlayerMove _player;

    void Start()
    {

    }

    void Update()
    {
        // Disable player movement if a conversation between two characters is active
        if (_dialogueController.conversationIsActive)
        {
            _player.enabled = false;
        }
        else
        {
            _player.enabled = true;
        }
    }
}
