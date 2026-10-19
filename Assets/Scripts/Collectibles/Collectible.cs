using System.Collections;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(AudioClip))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource playerAudio;

    private void Start()
    {
        playerAudio = GameObject.Find("Player SFX").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetCollected();
            Locator.Instance.LevelManager.PlayerCollectedStamp(GetComponent<Collectible>() );
        }
    }

    void GetCollected()
    {
        ObjectTypeCounter.InvokeObjectRemoved();
        playerAudio.clip = collectSound;
        playerAudio.Play();
    }

}

[MessagePackObject]
public class SerializableCollectible
{
    [Key(0)] public Vector3 collectiblePosition;

    // When saving, this constructor will be called to convert the game object into something that can be saved into a file
    public SerializableCollectible(Collectible collectible)
    {
        collectiblePosition = collectible.transform.position;
    }

    // When loading, this constructor wil be called b/c we are deserializing the serialized collectible object;
    // the conversion from serialized object into a game object will have to be handled elsewhere
    [SerializationConstructor]
    public SerializableCollectible(Vector3 savedPos)
    {
        collectiblePosition = savedPos;
    }
}
