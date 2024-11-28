using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColourGuideToggle : MonoBehaviour
{
    public GameObject colourGuidePanel; // Reference to the guide panel

    [SerializeField] private List<SpriteRenderer> images;

    private bool isGuideVisible = false;

    public void ToggleColourGuide()
    {
        isGuideVisible = !isGuideVisible; // Toggle the visibility state
        colourGuidePanel.SetActive(isGuideVisible); // Show or hide the panel

        SetupColors(); // Setup preferred colors
    }

    void SetupColors()
    {
        List<Color> colors;
        if (SettingsManager.Instance.GetColors().Count != 0)
        {
           colors = new List<Color>(SettingsManager.Instance.GetColors());
        }
        else
        {
            colors = new List<Color>(SettingsManager.Instance.GetDefaultColors());
        }

        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = colors[i];
        }
    }
}
