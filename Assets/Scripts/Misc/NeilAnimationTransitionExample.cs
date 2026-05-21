using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilAnimationTransitionExample : MonoBehaviour
{
    [SerializeField] private RectTransform circle; 

    private PlayerMove _playerMove;

    public void Init(PlayerMove move)
    {
        _playerMove = move;
        circle.transform.position = _playerMove.transform.position;

    }

    public void SetCircleCenter(Vector2 center)
    {
        circle.position = center;
    }

    private void Update()
    {
        if (_playerMove == null) return;

        circle.transform.position = _playerMove.transform.position;
    }
}
