using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] NeilAnimationTransitionExample SceneTransitionAnimationPrefab;
    private void CircleTransition(Vector2 closePoint)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(closePoint);
        var animation = Instantiate(SceneTransitionAnimationPrefab, Camera.main.transform.position, Quaternion.identity, this.gameObject.transform);

    }
}
