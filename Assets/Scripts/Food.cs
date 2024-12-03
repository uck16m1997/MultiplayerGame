using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Food : NetworkBehaviour
{
    public GameObject FoodPrefab;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(FoodPrefab == null) return; 
        
        if(!NetworkManager.Singleton.IsServer) return;
        if (other.TryGetComponent(out PlayerLength playerLength)) {
            playerLength.AddLength();
        }
        else if (other.TryGetComponent(out Tail tail)) {
            tail.NetworkedOwner.GetComponent<PlayerLength>().AddLength();
        }
        

        // Destroy the food
        if(NetworkObject.IsSpawned) NetworkObject.Despawn();
        else {
            Debug.Log("Uh Oh");
        }
        if(NetworkObject.isActiveAndEnabled) NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject,FoodPrefab);
        else {
            Debug.Log("Uh Oh");
        }

    }
}
