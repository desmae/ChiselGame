using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;

/*
     * RewardOptionUI.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-17
     * 
     * Description: This script is in charge of an individual reward's display within the reward UI.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-17
     * 
     *   -> 1.0 - Created RewardOptionUI.cs and hooked it to GameLoopManager.cs to be used there.
     *
     *   v1.0
     */
public class RewardOptionUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Button selectButton;

    private PowerUp myPowerUp;
    private Action<PowerUp> onSelectedCallback;

    public void Setup(PowerUp powerUp, Action<PowerUp> callback)
    {
        myPowerUp = powerUp;
        onSelectedCallback = callback;

        iconImage.sprite = powerUp.Icon;
        nameText.text = powerUp.Name;
        descriptionText.text = powerUp.Description;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnOptionSelected());
    }

    private void OnOptionSelected()
    {
        onSelectedCallback?.Invoke(myPowerUp);
    }
}

