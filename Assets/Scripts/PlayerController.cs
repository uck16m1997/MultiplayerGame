using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    private Camera _mainCamera;
    private Vector3 _mouseInput = Vector3.zero;


    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Initialize() {
        _mainCamera = Camera.main;
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

    private void ClientAuthoratative() {

    }
}
