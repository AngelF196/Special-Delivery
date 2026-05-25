using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static GameManager;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] NeilAnimationTransitionExample SceneTransitionAnimationPrefab;
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        gameManager.sceneTransition.AddListener(OnSceneTransition);
    }

    private void OnDisable()
    {
        gameManager.sceneTransition.RemoveListener(OnSceneTransition);
    }

    private void OnSceneTransition(SceneTransition transition, Vector2 position)
    {
        switch (transition) {
            case GameManager.SceneTransition.circle:
                CircleTransition(position);
                break;
            case GameManager.SceneTransition.slide:
                break;
            default:
                break;


        }
    }



    private void CircleTransition(Vector2 closePoint)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(closePoint);
        var animation = Instantiate(SceneTransitionAnimationPrefab, Camera.main.transform.position, Quaternion.identity, this.gameObject.transform);

    }

    private void OnSceneLoaded()
    {

    }
}
