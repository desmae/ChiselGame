using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
* ColorPickers.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: Code used in adjusting and previewing the colors set for blocks
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2024-10-10
* 
* 
*   -> 1.0 - Created ColorPickers.cs and implemented basic UI sliders to control block colors.
*   Default colors automatically set on startup.
*   
*   v1.0
*/
public class ColorPickers : MonoBehaviour
{
    [SerializeField] List<GameObject> sliders = new List<GameObject>();
    [SerializeField] List<RawImage> previews = new List<RawImage>();

    List<Color> colors = new List<Color>();

    private void Update()
    {
        for (int i = 0; i < sliders.Count; i += 3)
        {
            float redVal = sliders[i].GetComponent<Slider>().value;
            float greVal = sliders[i+1].GetComponent<Slider>().value;
            float bluVal = sliders[i+2].GetComponent<Slider>().value;

            int previewIndex = Mathf.FloorToInt(i / 3);
            colors[previewIndex] = previews[previewIndex].color;
            previews[previewIndex].color = CalculateColor(redVal, greVal, bluVal);
        }
    }

    Color CalculateColor(float r, float g, float b)
    {
        return new Color(r, g, b);
    }

    public List<Color> GetColors()
    {
        return colors;
    }

    public void SetDefaultColors(List<Color> colors)
    {
        this.colors = colors;
        AssignSliderValues(colors);
    }

    void AssignSliderValues(List<Color> colors)
    {
        for (int i = 0; i < sliders.Count; i += 3)
        {
            int colorIndex = Mathf.FloorToInt(i / 3);

            float redVal = colors[colorIndex].r;
            float greVal = colors[colorIndex].g;
            float bluVal = colors[colorIndex].b;

            sliders[i].GetComponent<Slider>().value = redVal;
            sliders[i + 1].GetComponent<Slider>().value = greVal;
            sliders[i + 2].GetComponent<Slider>().value = bluVal;
        }
    }
}
