using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTilt : MonoBehaviour
{
    private PlayerMove movement;
    private Rigidbody2D rb;
    private PlayerBoost boost;

    [SerializeField] private int tiltUpAmount;
    [SerializeField] private int tiltDownAmount;

    private float currentTilt;
    private bool diveJustStarted = true;

    private void Start()
    {
        movement = GetComponentInParent<PlayerMove>();
        boost = GetComponentInParent<PlayerBoost>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (movement.currentState == PlayerMove.state.diving || movement.currentState == PlayerMove.state.bonked)
        {
            DiveTilt();
            diveJustStarted = false;
        }
        else if (transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.identity;
            diveJustStarted = true;
        }
    }

    private void DiveTilt()
    {
        Vector2 vel = rb.velocity;

        if (vel.sqrMagnitude < 0.01f) return;

        float motionAngle = Mathf.Atan2(vel.y, Mathf.Abs(vel.x)) * Mathf.Rad2Deg;

        float targetTilt = motionAngle * 0.6f;
        targetTilt = Mathf.Clamp(targetTilt, tiltDownAmount, tiltUpAmount);

        if (movement.isFacingLeft) targetTilt = -targetTilt;

        // Snap on first frame of dive
        if (diveJustStarted)
        {
            currentTilt = targetTilt;
            diveJustStarted = false;
        }
        else
        {
            currentTilt = Mathf.LerpAngle(currentTilt, targetTilt, 12f * Time.deltaTime);
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }
}
