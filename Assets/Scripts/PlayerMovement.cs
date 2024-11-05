using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour {
    
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _movement = new();
    
    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner) {
            GetComponent<PlayerInput>().enabled = true;
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner) return;
        _rb.linearVelocity = _movement * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context) {
        if (!IsOwner) return;
        var direction = context.ReadValue<Vector2>();
        _movement = direction * moveSpeed;
    }

    /*[Rpc(SendTo.Server)]
    private void MoveRpc(Vector2 direction, RpcParams rpcParams = default) {
        // server validates movement
        if (rpcParams.Receive.SenderClientId != OwnerClientId) {
            Debug.LogWarning($"Client {rpcParams.Receive.SenderClientId} attempted to control a player ({OwnerClientId}) they do not own.");
            return;
        }
    }*/
}