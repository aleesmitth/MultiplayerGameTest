using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        _rb.linearVelocity = _movement * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context) {
        _movement = context.ReadValue<Vector2>();
    }
}