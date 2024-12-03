using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private float _foodCount = 30f;
    [SerializeField] private float _maxFoodCount = 30f;

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
    }

    private void SpawnFoodStart() {
        NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
        // NetworkObjectPool.Singleton.InitializePool();

        for (int i = 0; i < _foodCount; i++) {
            SpawnFood();
        }

        StartCoroutine(SpawnFoodOverTime());
    }

    private IEnumerator SpawnFoodOverTime() {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0) {
            yield return new WaitForSeconds(2f);
            if( NetworkObjectPool.Singleton.ReturnCurrentPooledObjectCount(_foodPrefab) >= _maxFoodCount) continue;
            
            NetworkObject obj =  NetworkObjectPool.Singleton.GetNetworkObject(_foodPrefab, GetRandomPositionOnMap(), Quaternion.identity);
            obj.GetComponent<Food>().FoodPrefab = _foodPrefab;
            obj.Spawn(true);
        }
    }

    private void SpawnFood() {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(_foodPrefab, GetRandomPositionOnMap(), Quaternion.identity);

        obj.GetComponent<Food>().FoodPrefab = _foodPrefab;
        obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap() {
        return new Vector3(Random.Range(-9f, 9), Random.Range(-5, 5), 0);
    }
    
}
