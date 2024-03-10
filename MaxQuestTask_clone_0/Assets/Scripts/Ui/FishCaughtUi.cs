using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishCaughtUi : MonoBehaviour
{
    [SerializeField] private Image fishImage;
    [SerializeField] private TextMeshProUGUI fishNameText;

    public void SetupFish(Sprite fishIcon, string fishName)
    {
        fishImage.sprite = fishIcon;
        fishNameText.text = fishName;
        Destroy(gameObject, 2f);
    }
}
