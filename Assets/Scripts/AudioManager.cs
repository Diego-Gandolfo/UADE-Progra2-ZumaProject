using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClips
{
    MouseClick,
    Win,
    Gameover,
    Shoot,
    Absorb,
    Explosion
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField, Range(0, 1)] private float musicInitialVolumen;
    [SerializeField] private AudioClip music;

    [Header("Sounds")]
    [SerializeField] private AudioSource soundsAudioSource;
    [SerializeField] private AudioClip mouseClick;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip gameover;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip absorb;
    [SerializeField] private AudioClip explosion;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        musicAudioSource.volume = musicInitialVolumen;
        musicAudioSource.clip = music;
        musicAudioSource.Play();
    }

    public void PlaySound(SoundClips soundClip)
    {
        switch (soundClip)
        {
            case SoundClips.MouseClick:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(mouseClick);
                break;
            case SoundClips.Win:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(win);
                break;
            case SoundClips.Gameover:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(gameover);
                break;
            case SoundClips.Shoot:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(shoot);
                break;
            case SoundClips.Absorb:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(absorb);
                break;
            case SoundClips.Explosion:
                soundsAudioSource.volume = 1f;
                soundsAudioSource.PlayOneShot(explosion);
                break;
            default:
                break;
        }
    }
}
