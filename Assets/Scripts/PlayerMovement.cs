using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour {
    
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private readonly NetworkVariable<Vector2> _movement = new NetworkVariable<Vector2>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        _rb.linearVelocity = _movement.Value * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context) {
        if (IsOwner) {
            var direction = context.ReadValue<Vector2>();
            MoveRpc(direction);
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveRpc(Vector2 direction, RpcParams rpcParams = default) {
        _movement.Value = direction * moveSpeed;
    }
}