using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishCatcher : NetworkBehaviour
{
    [SerializeField] private List<FishType> allFishes;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private FishCaughtUi fishCaughtUiPrefab;
    [SerializeField] private GameObject fishMissedUiPrefab;
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
        var fishMissed = Instantiate(fishMissedUiPrefab, uiCanvas.transform);
        Destroy(fishMissed, 1f);
    }

    private void FishCaught()
    {
        var fish = CatchRandomFish();
        var fishUi = Instantiate(fishCaughtUiPrefab, uiCanvas.transform);
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
        RefreshUiServerRpc(totalFishCaught, totalAttempts);
        RefreshUiClientRpc(totalFishCaught, totalAttempts);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RefreshUiServerRpc(int totalFishCaught, int totalAttempts)
    {
        fishCaughtUiText.text = totalFishCaught.ToString();
        pullAttemptsUiText.text = totalAttempts.ToString();
    }

    [ClientRpc]
    private void RefreshUiClientRpc(int totalFishCaught, int totalAttempts)
    {
        fishCaughtUiText.text = totalFishCaught.ToString();
        pullAttemptsUiText.text = totalAttempts.ToString();
    }
}
