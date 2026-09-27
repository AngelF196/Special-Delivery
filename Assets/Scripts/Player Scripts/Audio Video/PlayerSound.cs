using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource Audio;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip flipSound;
    public AudioClip diveSound;
    public AudioClip boostSound;
    public AudioClip handSpringSound;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    void PlaySound (AudioClip sound)
    {
        Audio.PlayOneShot(sound);

    }

}
