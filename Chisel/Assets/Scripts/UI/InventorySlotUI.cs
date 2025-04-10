using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image slotImage;          // The icon image
    public HoverableObject hoverable; // The HoverableObject for name/desc

    public void SetPowerUp(PowerUp powerUp)
    {
        slotImage.sprite = powerUp.Icon;
        slotImage.enabled = true;
        slotImage.color = powerUp.powerUpColor;

        hoverable.DisplayName = powerUp.Name;
        hoverable.Description = powerUp.Description;
    }

    public void SetEmpty()
    {
        slotImage.sprite = null;
        slotImage.enabled = false;

        hoverable.DisplayName = "";
        hoverable.Description = "";
    }
}

