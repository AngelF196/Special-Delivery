using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FileSaveData  // Doesn't inherit from MonoBehaviour b/c it's not a game object to instantiate
{
    // public Vector3 playerPosition;  // CANNOT be done b/c binary serialization doesn't support Unity specific stuff, only primitive types
    public float[] playerPosition;

    public FileSaveData(PlayerMove player)
    {
        playerPosition = new float[3];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
    }
}
