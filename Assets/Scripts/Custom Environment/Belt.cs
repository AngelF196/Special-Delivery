using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private float _force = 2f;
    [SerializeField] private bool movingRightwards = true;

    private Vector2 movement;

    //private void Start()
    //{
    //    SpriteRenderer Arrow = GetComponentInChildren<SpriteRenderer>();
    //    if (!movingRightwards) 
    //    {
    //        Arrow.flipY = false;
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (movingRightwards)
        {
            movement = Vector2.right * _force * Time.deltaTime;
            Debug.Log("trying to push right");
        }
        if (!movingRightwards)
        {
            movement = Vector2.left * _force * Time.deltaTime;
            Debug.Log("trying to push left");
        }
        collision.transform.Translate(movement);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(movement);
        Debug.Log("trying to push player");
    }
}
