using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager Instance { get; private set; }

    [Header("Références - Ne pas modifier")]
    public AudioSource playerAudioSource;
    public AudioSource playerAudioSourceSFX;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySoundOnPlayerAudioSource()
    {
        if (playerAudioSource.clip != null)
        {
            playerAudioSource.Play();
        }
    }

    public void PlaySoundOnPlayerAudioSource(AudioClip clip)
    {
        playerAudioSource.clip = clip;
        playerAudioSource.Play();
    }

    public void StopSoundOnPlayerAudioSource()
    {
        if (playerAudioSource.isPlaying)
        {
            playerAudioSource.Stop();
        }
    }

    public void PlaySoundOnPlayerAudioSourceSFX()
    {
        if (playerAudioSourceSFX.clip != null)
        {
            playerAudioSourceSFX.Play();
        }
    }

    public void PlaySoundOnPlayerAudioSourceSFX(AudioClip clip)
    {
        playerAudioSourceSFX.clip = clip;
        playerAudioSourceSFX.Play();
    }

    public void StopSoundOnPlayerAudioSourceSFX()
    {
        if (playerAudioSourceSFX.isPlaying)
        {
            playerAudioSourceSFX.Stop();
        }
    }
}
