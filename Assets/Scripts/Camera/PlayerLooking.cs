using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviour
{
    
    private PlayerInput input;
    [SerializeField] private float lookDistance;

    void Start()
    {
        input = GameObject.Find("player").GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.localPosition = new Vector3(input.look.x * lookDistance, input.look.y * lookDistance, 0);
    }
}
