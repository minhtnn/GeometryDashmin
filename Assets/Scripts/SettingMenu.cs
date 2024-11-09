using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    
    public AudioManager audioManager;
    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        musicSlider.value = audioManager.AudioVolume;
        SFXSlider.value = audioManager.SFXVolume;
    }
    public void SetBackgroundVolumn()
    {
        float volumn = musicSlider.value;
        audioManager.AudioVolume = volumn;
    }
    public void SetSFXVolumn()
    {
        float volumn = SFXSlider.value;
        audioManager.SFXVolume = volumn;
    }
}
