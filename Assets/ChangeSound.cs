using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSound : MonoBehaviour
{
    // Master Sound Variables
    public AudioListener Audiolistener;
    public Toggle masterToggle;
    public Slider masterVolumeSlider;

    // Music Sound Variables
    public AudioSource Music;
    public Toggle musicToggle;
    public Slider musicVolumeSlider;

    // Sound Effects Variables
    public AudioSource soundEffectsManager;
    public Toggle soundEffectsToggle;
    public Slider soundEffectsVolumeSlider;

    void Start()
    {
        Music = Music.GetComponent<AudioSource>();
    }

    public void ToggleMaster()
    {
        if(masterToggle.isOn)
        {
            Audiolistener.GetComponent<AudioListener>().enabled = true;
        } else {
            Audiolistener.GetComponent<AudioListener>().enabled = false;
        }
    }

    public void MasterVolume()
    {
        AudioListener.volume = masterVolumeSlider.value;
    }

    public void ToggleMusic()
    {
        if(musicToggle.isOn)
        {
            Music.mute = false;
        } else {
            Music.mute = true;
        }
    }

    public void MusicVolume()
    {
        Music.volume = musicVolumeSlider.value;
    }

    public void ToggleSoundEffects()
    {
        if(soundEffectsToggle.isOn)
        {
            soundEffectsManager.mute = false;
        } else {
            soundEffectsManager.mute = true;
        }
    }

    public void SoundEffectsVolume()
    {
        soundEffectsManager.volume = soundEffectsVolumeSlider.value;
    }
}
