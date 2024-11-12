using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* AudioAsset.cs
* Created by: Evan Robertson
* Date Created: 2024-11-11
* 
* Description: Scriptable Object for audio assets
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2024-11-11
* 
* 
*   -> 1.0 - Created AudioAsset.cs
*   
*   v1.0
*/
[CreateAssetMenu(menuName = "Audio/Audio Asset")]
public class AudioAsset : ScriptableObject
{
    public string AudioName;
    public AudioClip AudioFile;
    public bool IsLooping = false;
}
