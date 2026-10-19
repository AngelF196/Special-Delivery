using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EndPoint : MonoBehaviour
{
    public UnityEvent arrivedAtEnd = new UnityEvent();

    void Start()
    {
        arrivedAtEnd.AddListener(Locator.Instance.QuestManager.QuestEnded);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        arrivedAtEnd.Invoke();
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
