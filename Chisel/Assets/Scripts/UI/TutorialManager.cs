using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // The tutorial panel
    public TextMeshProUGUI tutorialText; // The text component for displaying tutorial pages
    public string[] tutorialPages; // An array to hold all tutorial pages
    private int currentPage = 0; // Keeps track of the current tutorial page

    void Start()
    {
        // Initialize tutorial
        if (tutorialPages.Length > 1)
        {
            ShowTutorialPage(3); // Show the first page
        }
        else
        {
            Debug.LogError("No tutorial pages found!");
        }
    }

    public void ShowNextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            ShowTutorialPage(currentPage);
        }
        else
        {
            CloseTutorial(); // Close if on the last page
        }
    }

    private void ShowTutorialPage(int pageIndex)
    {
        tutorialText.text = tutorialPages[pageIndex]; // Update the tutorial text
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false); // Hide the tutorial panel
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        currentPage = 0;
        ShowTutorialPage(0); // Reset to the first page when reopening
    }
}
