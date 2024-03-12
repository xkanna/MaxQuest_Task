using Unity.Netcode;
using UnityEngine;

public class FishManager : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnFishesServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnFishesServerRpc()
    {
        for (var i = 0; i < numberOfFishes; i++)
        {
            var fish = Instantiate(fishPrefab, transform.position, Quaternion.identity);
            var fishNetworkObject = fish.GetComponent<NetworkObject>();
            fishNetworkObject.Spawn();
        }
    }

    [ClientRpc]
    private void InstantiateNewFishClientRpc(Vector3 position)
    {
        var fish = Instantiate(fishPrefab, position, Quaternion.identity);
        fish.gameObject.SetActive(true);
    }

    public void SpawnFishLocally(GameObject fishToDestroy)
    {
        if (IsServer)
        {
            SpawnFishesServerRpc(Vector3.zero);
            fishToDestroy.GetComponent<NetworkObject>().Despawn();
        }
        else
        {
            InstantiateNewFishClientRpc(Vector3.zero);
        }
    }

    [ServerRpc]
    private void SpawnFishesServerRpc(Vector3 position)
    {
        var fish = Instantiate(fishPrefab, position, Quaternion.identity);
        fish.gameObject.SetActive(true);
        
        var fishNetworkObject = fish.GetComponent<NetworkObject>();
        fishNetworkObject.Spawn();
    }
    
}
