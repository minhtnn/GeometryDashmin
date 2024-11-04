using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------Audio source-------")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("--------Audio clip--------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip buttonSelection;
    [Header("--------Audio clip--------")]
    public float AudioVolume = 1.0f;
    public float SFXVolume = 1.0f;
    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (background)
        {
            musicSource.clip = background;
            musicSource.volume = AudioVolume;
            musicSource.Play();
        }
    }
    private void Update()
    {
        if (background)
            musicSource.volume = AudioVolume;
        if (death && buttonSelection)
            SFXSource.volume = SFXVolume;
    }
    public void PlaySFX(AudioClip clip, float volumn)
    {
        if (clip != null)
        {
            SFXVolume = volumn;
            SFXSource.PlayOneShot(clip, SFXVolume);
        }
    }

    public void StopBackgroundMusic()
    {
        if (background && musicSource.isPlaying) 
        {
            musicSource.Stop();
        }
    }

    public void PauseBackgroundMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void UnpauseBackgroundMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }
}
