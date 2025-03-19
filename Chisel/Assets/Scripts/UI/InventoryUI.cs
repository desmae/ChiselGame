using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
     * InventoryUI.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-18
     * 
     * Description: This script is in charge of subscribing to an event that will allow the inventory
     *              UI to refresh when a power up is added or removed.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-18
     *
     *  -> 1.0 - Created InventoryUI.cs and wrote the basic functionalities as mentioned in the description.
     *   v1.0
     */
public class InventoryUI : MonoBehaviour
{
    // we have exactly 12 slots in the UI
    public InventorySlotUI[] slots;

    private void OnEnable()
    {
        PowerUpManager.OnPowerUpListChanged += RefreshUI;
    }
    private void OnDisable()
    {
        PowerUpManager.OnPowerUpListChanged -= RefreshUI;
    }
    public void RefreshUI()
    {
        foreach (var slot in slots)
        {
            slot.SetEmpty();
        }

        for (int i = 0; i < PowerUpManager.activePowerUps.Count && i < slots.Length; i++)
        {
            PowerUp p = PowerUpManager.activePowerUps[i];
            slots[i].SetPowerUp(p);
        }
    }
}
