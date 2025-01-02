using System;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;


public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    private Camera _mainCamera;
    private Vector3 _mouseInput = Vector3.zero;
    private PlayerLength _playerLength;


    public override void OnNetworkSpawn() {
        Regex.Split()
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Initialize() {
        _mainCamera = Camera.main;
        _playerLength = GetComponent<PlayerLength>();
    }
    
    private void Update() {
        if(!IsOwner || !Application.isFocused) return;
        
        _mouseInput = Input.mousePosition;
        _mouseInput.z = _mainCamera.nearClipPlane;
        
        Vector3 mouseWorldCoordinates = _mainCamera.ScreenToWorldPoint(_mouseInput);
        mouseWorldCoordinates.z = 0;
        
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates,Time.deltaTime * _moveSpeed);

        if (mouseWorldCoordinates != transform.position) {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0;
            transform.up = targetDirection;
        }
    }

    [Rpc(SendTo.Server,RequireOwnership = true)]
    private void DetermineCollisionWinnerServerRpc(PlayerData player1,PlayerData player2)
    {
        if (player1.Length > player2.Length)
        {
            WinInfoServerRpc(player1.Id,player2.Id);
        }
        else
        {
            WinInfoServerRpc(player2.Id,player1.Id);
        }
    }

    [Rpc(SendTo.Server,RequireOwnership = true)]
    private void WinInfoServerRpc(ulong winner, ulong loser)
    {
        
    }

    [Rpc(SendTo.NotServer)]
    private void AtePlayerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if(!IsOwner) return;
        Debug.Log("You Ate a Player!");

    }

    [Rpc(SendTo.NotServer)]
    private void GameOverClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if(!IsOwner) return;
        Debug.Log("You Lose!");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (!IsOwner) return;

        // Head-on Collision
        if (col.gameObject.TryGetComponent(out PlayerLength playerLength))
        {
            var player1 = new PlayerData() { Id = OwnerClientId, Length = _playerLength.length.Value };
            var player2 = new PlayerData() { Id = playerLength.OwnerClientId, Length = playerLength.length.Value };
            DetermineCollisionWinnerServerRpc(player1,player2);
        }
    }

    struct PlayerData : INetworkSerializable
    {
        public ulong Id;
        public ushort Length;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Length);
        }
    }
}
