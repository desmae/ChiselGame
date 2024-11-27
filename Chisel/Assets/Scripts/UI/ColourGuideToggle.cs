using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGuideToggle : MonoBehaviour
{
    public GameObject colourGuidePanel; // Reference to the guide panel

    private bool isGuideVisible = false;

    public void ToggleColourGuide()
    {
        isGuideVisible = !isGuideVisible; // Toggle the visibility state
        colourGuidePanel.SetActive(isGuideVisible); // Show or hide the panel
    }
}
