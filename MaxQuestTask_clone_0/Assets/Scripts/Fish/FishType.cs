using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishType_000", menuName = "FishType")]
public class FishType : ScriptableObject
{
    [SerializeField] private string fishName;
    [SerializeField] private float chanceOfCatching;
    [SerializeField] private Sprite fishIcon;
    
    public string FishName => fishName;
    public float ChanceOfCatching => chanceOfCatching;
    public Sprite FishIcon => fishIcon;
}
