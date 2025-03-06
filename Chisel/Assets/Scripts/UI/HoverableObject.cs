using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
     * HoverableObject.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-08
     * 
     * Description: This code can be attached to objects to make them interactable with.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-08
     * 
     * 
     *   -> 1.0 - Created HoverableObject.cs and created baseline code to be used on
     *          hoverable objects that will provide info when right-clicked.
     *   v1.0
     */
public class HoverableObject : MonoBehaviour
{
    public string DisplayName = "Untitled";
    [TextArea] public string Description = "Untitled";

    private void OnMouseExit()
    {
        HoverManager.Instance.HideInfoCard();
    }
}
