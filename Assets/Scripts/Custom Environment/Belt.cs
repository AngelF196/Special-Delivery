using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private float _force = 2f;
    [SerializeField] private bool movingRightwards = true;
    [SerializeField] private GameObject Arrow;

    private Vector2 movement;

    private void Start()
    {
        Debug.Log(transform.localEulerAngles.z);
    }

    private void Update ()
    {
        if (!movingRightwards) 
        {
            Arrow.gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
        else
        {
            Arrow.gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.localEulerAngles.z == 0f)
        {
            if (movingRightwards)
            {
                movement = Vector2.right * _force * Time.deltaTime;
            }
            if (!movingRightwards)
            {
                movement = Vector2.left * _force * Time.deltaTime;
            }
        }

        if (transform.localEulerAngles.z == 90f)
        {
            if (movingRightwards)
            {
                movement = Vector2.up * _force * Time.deltaTime;
            }
            if (!movingRightwards)
            {
                movement = Vector2.down * _force * Time.deltaTime;
            }
        }

        if (transform.localEulerAngles.z == 270f)
        {
            if (movingRightwards)
            {
                movement = Vector2.down * _force * Time.deltaTime;
            }
            if (!movingRightwards)
            {
                movement = Vector2.up * _force * Time.deltaTime;
            }
        }

        collision.transform.Translate(movement);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(movement);
    }
}
