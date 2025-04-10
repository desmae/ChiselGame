using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CorruptedGemOptionUI : MonoBehaviour
{
    [Header("UI References")]
    public Image rewardImage;
    public TextMeshProUGUI rewardName;
    public TextMeshProUGUI buffText;
    public TextMeshProUGUI nerfText;
    public Button selectButton;

    private CorruptedGem myGem;
    private Action<CorruptedGem> onSelectedCallback;

    public void Setup(CorruptedGem gem, Action<CorruptedGem> callback)
    {
        myGem = gem;
        onSelectedCallback = callback;

        rewardImage.sprite = gem.Icon;
        rewardName.text = gem.Name;
        rewardImage.color = gem.Color; 

        buffText.text = gem.BuffDescription;
        nerfText.text = gem.NerfDescription;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => OnGemSelected());
    }

    private void OnGemSelected()
    {
        onSelectedCallback?.Invoke(myGem);
    }
}
