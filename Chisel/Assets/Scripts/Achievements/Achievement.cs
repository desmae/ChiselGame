using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Achievement
{
    public string title;
    public string description;
    public bool unlocked;
    public int currentValue;
    public int requiredValue;
}
