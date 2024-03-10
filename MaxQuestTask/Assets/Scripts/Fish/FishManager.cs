using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private Fish fishPrefab;
    [SerializeField] private int numberOfFishes;
    public static FishManager Instance { get; private set; }

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        for (int i = 0; i < numberOfFishes; i++)
        {
            Instantiate(fishPrefab, transform);
        }
    }

    public void InstantiateNewFish()
    {
        Instantiate(fishPrefab, transform);
    }
    
}
