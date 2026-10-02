using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    PlayerInfo playerInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.tag.ToString() == "Player")
        {
            playerInfo = collision.transform.parent.gameObject.GetComponent<PlayerInfo>();
            playerInfo.UpdateHook(transform.position);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.tag.ToString() == "Player")
        {
            playerInfo.UpdateHook(transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.tag.ToString() == "Player")
        {
            playerInfo.UpdateHook(Vector3.zero);
            playerInfo = null;
        }
    }
}
