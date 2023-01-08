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
    private AudioSource Music;
    public Toggle musicToggle;
    public Slider musicVolumeSlider;

    // Sound Effects Variables
    public AudioSource soundEffectsManager;
    public Toggle soundEffectsToggle;
    public Slider soundEffectsVolumeSlider;

    public CurrentSettings currentSettings;

    void Update()
    {
        Audiolistener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();

        try
        {
            Music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        } catch {
            Music = GameObject.FindGameObjectWithTag("LevelMusic").GetComponent<AudioSource>();
        }

        try
        {
            soundEffectsManager = GameObject.FindGameObjectWithTag("SoundEffects").GetComponent<AudioSource>();
        } catch {
            soundEffectsManager = GameObject.FindGameObjectWithTag("LevelSoundEffects").GetComponent<AudioSource>();
        }

        try
        {
            currentSettings = GameObject.FindGameObjectWithTag("CurrentSettings").GetComponent<CurrentSettings>();
        } catch {
            currentSettings = GameObject.FindGameObjectWithTag("LevelCurrentSettings").GetComponent<CurrentSettings>();
        }

        currentSettings.changeSound = this.gameObject.GetComponent<ChangeSound>();
    }

    public void ToggleMaster()
    {
        if(masterToggle.isOn)
        {
            Audiolistener.GetComponent<AudioListener>().enabled = true;
            currentSettings.MasterOn = true;
        } else {
            Audiolistener.GetComponent<AudioListener>().enabled = false;
            currentSettings.MasterOn = false;
        }
    }

    public void MasterVolume()
    {
        AudioListener.volume = masterVolumeSlider.value;
        currentSettings.MasterVolume = masterVolumeSlider.value;
    }

    public void ToggleMusic()
    {
        if(musicToggle.isOn)
        {
            Music.mute = false;
            currentSettings.MusicMute = false;
        } else {
            Music.mute = true;
            currentSettings.MusicMute = true;
        }
    }

    public void MusicVolume()
    {
        Music.volume = musicVolumeSlider.value;
        currentSettings.MusicVolume = musicVolumeSlider.value;
    }

    public void ToggleSoundEffects()
    {
        if(soundEffectsToggle.isOn)
        {
            soundEffectsManager.mute = false;
            currentSettings.SoundEffectsMute = false;
        } else {
            soundEffectsManager.mute = true;
            currentSettings.SoundEffectsMute = true;
        }
    }

    public void SoundEffectsVolume()
    {
        soundEffectsManager.volume = soundEffectsVolumeSlider.value;
        currentSettings.SoundEffectsVolume = soundEffectsVolumeSlider.value;
    }
}
