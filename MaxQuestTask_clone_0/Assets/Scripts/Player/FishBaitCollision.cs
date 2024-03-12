using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FishBaitCollision : MonoBehaviour
{
    public Action<bool> OnFishInRange;
    private List<GameObject> fishesInRange;

    private void Start()
    {
        fishesInRange = new List<GameObject>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CatchFishServerRpc()
    {
        if (fishesInRange.Count > 0)
        {
            var firstFish = fishesInRange[0].gameObject;
            fishesInRange.Remove(firstFish);

            // Spawn a new fish on all clients
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
