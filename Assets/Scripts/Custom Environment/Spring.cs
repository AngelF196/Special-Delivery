using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float bounce = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.localEulerAngles.z == 0f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            }
            if (transform.localEulerAngles.z == 90f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bounce, ForceMode2D.Impulse);
            }
            if (transform.localEulerAngles.z == 180f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bounce, ForceMode2D.Impulse);
            }
            if (transform.localEulerAngles.z == 270f)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bounce, ForceMode2D.Impulse);
            }

            // 1. create a rotation matrix using Quaternion.Euler from the spring's z-axis rotation
            // Use gizmos to draw the vector
            // https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Quaternion.Euler.html
            // multiply this rotation matrix * Vector3(0,1,0) to get your rotated vector
            // make a gizmo to check if this vector is correct
            // apply vector to player velocity
        }
    }
}
