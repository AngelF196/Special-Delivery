using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private PlayerInput playerInput;
    private PlayerBoost playerBoost;
    private Rigidbody2D rb;

    [SerializeField] private bool moving = false;
    [SerializeField] private float time = 0;
    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        playerInput = GetComponent<PlayerInput>();
        playerBoost = GetComponent<PlayerBoost>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerInput.saysTest)
        {
            Hookshoot();
            playerInput.Consume(PlayerInput.Action.test);
        }
        if (moving) 
        { 
            time -= Time.deltaTime;
            if (time <= 0) Hookstop();
        }
    }

    private void Hookshoot()
    {
        Debug.Log("test");
        if (playerInfo.hookPoint == Vector3.zero) return;

        Vector2 DistDiff = new Vector2(playerInfo.hookPoint.x - transform.position.x,
            playerInfo.hookPoint.y - transform.position.y);

        float distance = Mathf.Sqrt((DistDiff.x*DistDiff.x)+(DistDiff.y*DistDiff.y));
        float angle = Mathf.Atan2(DistDiff.y, DistDiff.x);

        Debug.Log($"Angle in Degrees: {angle}, Distance: {distance}");

        float snappedAngle = Mathf.Round(angle/(Mathf.PI/4)) * Mathf.PI/4;
        Debug.Log($"Closest angle: {snappedAngle}");
        HookMove(snappedAngle, distance);
    }

    private void HookMove(float snappedAngle, float distance)
    {
        int speed = Mathf.RoundToInt(playerBoost.CurrentMaxSpeed() - 2);
        time = distance / speed;

        Vector2 direction = new Vector2(Mathf.Cos(snappedAngle), Mathf.Sin(snappedAngle));
        rb.gravityScale = 0;
        moving = true;
        rb.velocity = direction * speed;
    }

    private void Hookstop()
    {
        rb.gravityScale = 2;
        moving = false;
    }
}
