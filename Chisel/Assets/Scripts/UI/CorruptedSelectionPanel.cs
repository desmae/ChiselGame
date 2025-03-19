using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CorruptedSelectionPanel : MonoBehaviour
{
    // Each slot is like your Reward1, Reward2, Reward3 in the hierarchy
    public CorruptedGemOptionUI[] gemSlots;

    // This callback will be invoked when the user selects one of the gems
    private Action<CorruptedGem> onGemSelected;

    public void SetupCorruptedGems(CorruptedGem[] gems, Action<CorruptedGem> callback)
    {
        onGemSelected = callback;

        // Make sure this panel is active
        gameObject.SetActive(true);

        for (int i = 0; i < gemSlots.Length; i++)
        {
            if (i < gems.Length)
            {
                gemSlots[i].gameObject.SetActive(true);
                gemSlots[i].Setup(gems[i], OnGemClicked);
            }
            else
            {
                // In case there are fewer than gemSlots
                gemSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnGemClicked(CorruptedGem chosenGem)
    {
        // The user clicked a gem. Invoke the callback, hide this panel.
        onGemSelected?.Invoke(chosenGem);
        gameObject.SetActive(false);
    }
}
