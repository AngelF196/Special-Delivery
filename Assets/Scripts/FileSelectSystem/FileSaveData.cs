using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

[MessagePackObject]
public class FileSaveData  // Doesn't inherit from MonoBehaviour b/c it's not a game object to instantiate AND MessagePack will complain that MonoBehaviour isn't a MessagePackObject (which is true :( )
{
    [Key(0)] public Vector3 playerPosition;

    // Creating data to save
    public FileSaveData(PlayerMove player)
    {
        playerPosition = player.transform.position;
    }

    // Load data from a save file (ordering of constructor arguments must match the keyed properties ordering)
    [SerializationConstructor]
    public FileSaveData(Vector3 savedPos)
    {
        playerPosition = savedPos;
    }
}
