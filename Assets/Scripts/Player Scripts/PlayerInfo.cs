using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private Vector3 _respawnPoint = Vector3.zero;
    public Vector3 respawnPoint => _respawnPoint;

    void Start()
    {
        _respawnPoint = transform.position;
    }

    public void UpdateRespawn(Vector3 newPoint)
    {
        _respawnPoint = newPoint;
    }
}
