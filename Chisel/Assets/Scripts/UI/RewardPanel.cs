using UnityEngine;
using System;
using UnityEngine.UI;
/*
     * RewardPanel.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-17
     * 
     * Description: This script is in charge of the Reward Screen UI, where players are able to collect their reward before going to the next level.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-17
     * 
     *   -> 1.0 - Created RewardPanel.cs and hooked it to GameLoopManager.cs to be used there.
     *
     *   v1.0
     */
public class RewardPanel : MonoBehaviour
{
    public RewardOptionUI[] optionSlots; 

    private Action<PowerUp> onPowerUpSelected;

    public void SetupRewardOptions(PowerUp[] powerUps, Action<PowerUp> callback)
    {
        onPowerUpSelected = callback;

        gameObject.SetActive(true);

        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i < powerUps.Length)
            {
                optionSlots[i].gameObject.SetActive(true);
                optionSlots[i].Setup(powerUps[i], OnOptionSelected);
            }
            else
            {
                optionSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnOptionSelected(PowerUp selectedPowerUp)
    {
        onPowerUpSelected?.Invoke(selectedPowerUp);

        gameObject.SetActive(false);
    }
}
