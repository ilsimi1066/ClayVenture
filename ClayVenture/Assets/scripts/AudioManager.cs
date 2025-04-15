using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip home;
    public AudioClip sewers;
    public AudioClip outside;
    public AudioClip checkpoint;
    public AudioClip death;
    public AudioClip gameover;
    public AudioClip dry;
    public AudioClip mud;

    private void Start()
    {
        musicSource.clip = sewers;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
