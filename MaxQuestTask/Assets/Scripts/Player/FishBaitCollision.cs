using System;
using System.Collections;
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

    public void CatchFish()
    {
        var firstFish = fishesInRange[0].gameObject;
        fishesInRange.Remove(fishesInRange[0]);
        Destroy(firstFish);
        FishManager.Instance.InstantiateNewFish();
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
