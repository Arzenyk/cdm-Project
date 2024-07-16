using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandSoundPaddle : MonoBehaviour
{
    public AudioClip[] hitSounds;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomHitSound()
    {
        int randomIndex = Random.Range(0, hitSounds.Length);
        audioSource.clip = hitSounds[randomIndex];
        audioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            PlayRandomHitSound();
        }
    }
}
