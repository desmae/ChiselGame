using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TMP_Text achieveName;
    [SerializeField] private TMP_Text achieveValue;
    [SerializeField] private Slider achieveProgress;

    public void SetValues(string name, string value, int progress, int max)
    {
        achieveName.text = name;
        achieveValue.text = value;
        achieveProgress.value = Mathf.InverseLerp(0, max, progress);
    }
}
