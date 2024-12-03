using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLength : NetworkBehaviour
{
    [SerializeField] private GameObject _tailPrefab;
    
    public NetworkVariable<ushort> length = new(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public static event Action<ushort> ChangedLengthEvent;
    
    private List<GameObject> _tails;
    private Transform _lastTail;
    private Collider2D _collider;

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        _tails = new List<GameObject>();
        _lastTail = transform;
        _collider = GetComponent<Collider2D>();
        if(!IsServer) length.OnValueChanged += LengthChangedEvent;
        
        for(int i=0;i<length.Value; i++) InstantiateTail();
    }
    
    

    [ContextMenu("Add Length")]
    public void AddLength() {
        length.Value++;
        LengthChanged();
    }

    private void LengthChanged() {
        InstantiateTail();
        
        if(!IsOwner) return;
        ChangedLengthEvent?.Invoke(length.Value);
        ClientMusicPlayer.Instance.PlayAudioClip();
    }
    
    private void LengthChangedEvent(ushort prev,ushort cur) {
        LengthChanged();
    }

    private void InstantiateTail() {
        GameObject tailGO = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        tailGO.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
        
        if ( tailGO.TryGetComponent(out Tail tail)) {
            tail.NetworkedOwner = transform;
            tail.FollowTransform = _lastTail;
            _lastTail = tailGO.transform;
            
            Physics2D.IgnoreCollision(tailGO.GetComponent<Collider2D>(),_collider);
        }
        
        _tails.Add(tailGO);
        
    }

}
