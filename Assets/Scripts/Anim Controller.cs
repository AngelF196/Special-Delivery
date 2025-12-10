using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    GameObject PlayerObject;

    void Start()
    {
        PlayerObject = GameObject.Find("player");
        PlayerMove Player = PlayerObject.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
