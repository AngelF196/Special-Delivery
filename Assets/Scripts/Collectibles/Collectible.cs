using System.Collections;
using System.Collections.Generic;
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
        }
    }

    void GetCollected()
    {
        playerAudio.clip = collectSound;
        playerAudio.Play();
        Destroy(this.gameObject);
    }

}
