using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPages; // An array to hold all tutorial pages
    private int currentPage = 0; // Keeps track of the current tutorial page

    void Start()
    {
        // Initialize tutorial
        if (tutorialPages.Length > 1)
        {
            ShowTutorialPage(0); // Show the first page
        }
        else
        {
            Debug.LogError("No tutorial pages found!");
        }

        // Disable play until tutorial is finished
        BlockScript.canBreak = false;
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
            CloseTutorialPage(); // Close if on the last page
        }
    }

    private void ShowTutorialPage(int pageIndex)
    {
        tutorialPages[pageIndex].gameObject.SetActive(true); // Show next page
    }

    public void CloseTutorialPage()
    {
        tutorialPages[currentPage].gameObject.SetActive(false); // Hide prev page

        if (currentPage == tutorialPages.Length - 1)
        {
            BlockScript.canBreak = true;
        }
    }
}
