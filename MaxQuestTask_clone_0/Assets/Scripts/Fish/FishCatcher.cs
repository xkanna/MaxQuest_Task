using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishCatcher : MonoBehaviour
{
    [SerializeField] private List<FishType> allFishes;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private FishCaughtUi fishCaughtUi;
    [SerializeField] private GameObject fishMissedUi;
    [SerializeField] private TextMeshProUGUI fishCaughtUiText;
    [SerializeField] private TextMeshProUGUI pullAttemptsUiText;
    
    private int totalAttempts = 0;
    private int totalFishCaught = 0;
    private int attemptsInSet = 0; 
    private int fishCaughtInSet = 0;

    private void Start()
    {
        RefreshUi();
    }

    public void TryToPullFish()
    {
        totalAttempts++;
        attemptsInSet++;
        
        var randomValue = Random.value;

        if (randomValue <= 0.3f)
        {
            totalFishCaught++;
            fishCaughtInSet++;
            FishCaught();
        }
        else
        {
           FishMissed();
        }

        if (attemptsInSet == 10)
        {
            attemptsInSet = 0;
            fishCaughtInSet = 0;
        }
        
        RefreshUi();
    }

    private void FishMissed()
    {
        var fishMissed = Instantiate(fishMissedUi, uiCanvas.transform);
        Destroy(fishMissed, 1f);
    }

    private void FishCaught()
    {
        var fish = CatchRandomFish();
        var fishUi = Instantiate(fishCaughtUi, uiCanvas.transform);
        fishUi.SetupFish(fish.FishIcon, fish.FishName);
    }

    private FishType CatchRandomFish()
    {
        var totalChanceOfCatching = 0f;
        foreach (var fishType in allFishes)
        {
            totalChanceOfCatching += fishType.ChanceOfCatching;
        }

        var randomValue = Random.Range(0f, totalChanceOfCatching);

        var cumulativeChance = 0f;
        foreach (var fishType in allFishes)
        {
            cumulativeChance += fishType.ChanceOfCatching;
            if (randomValue <= cumulativeChance)
            {
                return fishType;
            }
        }

        return null;
    }

    private void RefreshUi()
    {
        fishCaughtUiText.text = totalFishCaught.ToString();
        pullAttemptsUiText.text = totalAttempts.ToString();
    }
}
