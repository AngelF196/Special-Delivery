using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;
using UnityEngine.Animations;

[MessagePackObject]
public class FileSaveData  // Doesn't inherit from MonoBehaviour b/c it's not a game object to instantiate AND MessagePack will complain that MonoBehaviour isn't a MessagePackObject (which is true :( )
{
    [Key(0)] public Vector3 playerPosition;
    [Key(1)] public List<SerializableCollectible> savedCollectibles = new List<SerializableCollectible>();

    // Little workaround I came up with to not throw an error
    public FileSaveData()
    {
        Debug.LogError("Cannot create a PlayerData object out of the default constructor.");
    }

    public FileSaveData(PlayerMove player, LevelManager levelManager)
    {
        playerPosition = player.transform.position;
        List<Collectible> leftoverStamps = levelManager.stamps;
        foreach (Collectible savedStamp in leftoverStamps)
        {
            SerializableCollectible serializedCollectible = new SerializableCollectible(savedStamp);
            savedCollectibles.Add(serializedCollectible);
        }
    }
}
