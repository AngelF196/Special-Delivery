using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLooking : MonoBehaviour
{
    
    private PlayerInput input;
    private Vector2 currentPos;
    private Rigidbody2D rb;
    private PlayerMove movement;
    [SerializeField] private float lookDistance;
    [SerializeField] private float lookSpeed;
    [SerializeField] private float lookAheadSpeed;
    [SerializeField] private float lookAheadDiv;

    void Start()
    {
        input = GetComponentInParent<PlayerInput>();
        movement = GetComponentInParent<PlayerMove>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.look.magnitude > 0.2f)
        {
            ManualLooking();
        }
        else 
        {
            LookAhead();
        }
        transform.localPosition = new Vector3(currentPos.x, currentPos.y, 0) * lookDistance;
    }
    void LookAhead()
    {
        Vector2 target = new Vector2(0f, 0f);
        float speed = 3f;
        if (Mathf.Abs(rb.velocity.x) > movement.baseMaxSpeed + 3)
        {
            float x = Mathf.Clamp(rb.velocity.x / lookAheadDiv, -1f, 1f); // tweak divisor
            target = new Vector2(x, 0f);
            speed = lookAheadSpeed;
        }
        currentPos = Vector2.Lerp(currentPos, target, speed * Time.deltaTime);
    }
    void ManualLooking()
    {
        currentPos = Vector2.Lerp(currentPos, input.look, lookSpeed * Time.deltaTime);
    }
}
