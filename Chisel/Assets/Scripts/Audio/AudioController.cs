using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* AudioController.cs
* Created by: Evan Robertson
* Date Created: 2024-11-11
* 
* Description: Controller for managing music and sound fx
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2024-11-11
* 
* 
*   -> 1.0 - Created AudioController.cs
*   
*   v1.0
*/
public class AudioController : PersistentSingleton<AudioController>
{
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private List<AudioAsset> audioList;

    private void Awake()
    {
        //Adds all AudioAssets in Resources/Audio to list
        AudioAsset[] loadedAudio = Resources.LoadAll<AudioAsset>("Audio");
        audioList.AddRange(loadedAudio);
    }

    public void PlaySFX(string name)
    {
        sfxPlayer.PlayOneShot(GetAsset(name).AudioFile);
    }

    public void PlayMusic(string name)
    {
        AudioAsset asset = GetAsset(name);

        musicPlayer.loop = asset.IsLooping;
        musicPlayer.clip = asset.AudioFile;
        musicPlayer.Play();
    }

    AudioAsset GetAsset(string name)
    {
        foreach (AudioAsset asset in audioList)
        {
            if (asset.AudioName == name) return asset;
        }

        return audioList[0];
    }

    public void SetSFXVolume(float vol)
    {
        sfxPlayer.volume = vol;
    }

    public void SetMusicVolume(float vol)
    {
        musicPlayer.volume = vol;
    }
}
