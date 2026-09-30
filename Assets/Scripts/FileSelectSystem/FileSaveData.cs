using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;
using UnityEngine.Animations;

[MessagePackObject]
public class FileSaveData  // Doesn't inherit from MonoBehaviour b/c it's not a game object to instantiate AND MessagePack will complain that MonoBehaviour isn't a MessagePackObject (which is true :( )
{
    [Key(0)] public Vector3 playerPosition;
    [Key(1)] public List<SerializableCollectible> collectiblesToCollect = new List<SerializableCollectible>();

    public FileSaveData(PlayerMove player, LevelManager levelManager)
    {
        playerPosition = player.transform.position;
        List<Collectible> leftoverStamps = levelManager.stamps;
        foreach (Collectible savedStamp in leftoverStamps)
        {
            SerializableCollectible serializedCollectible = new SerializableCollectible(savedStamp);
            collectiblesToCollect.Add(serializedCollectible);
        }
    }
    
    // When creating a PlayerData object from deserializing a file, the constructor with the SerializationConstructor field will be called 
    [SerializationConstructor]
    public FileSaveData(Vector3 savedPos, List<SerializableCollectible> savedStamps)
    {
        Debug.Log("MessagePack deserialization constructor called");
        playerPosition = savedPos;
        collectiblesToCollect = savedStamps;
    }
}
