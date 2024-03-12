using System;
using System.Collections.Generic;
using UnityEngine;

public class FishBaitCollision : MonoBehaviour
{
    public Action<bool> OnFishInRange;
    private List<GameObject> fishesInRange;

    private void Start()
    {
        fishesInRange = new List<GameObject>();
    }

    public void CatchFishServerRpc()
    {
        if (fishesInRange.Count > 0)
        {
            var firstFish = fishesInRange[0].gameObject;
            fishesInRange.Remove(firstFish);
            FishManager.Instance.SpawnFishLocally(firstFish);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            OnFishInRange.Invoke(true);
            fishesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            OnFishInRange.Invoke(false);
            fishesInRange.Remove(other.gameObject);
        }
    }
}
