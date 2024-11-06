using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerClicked : NetworkBehaviour {
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private string selectedLetter = "F";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("F key was pressed!");
            selectedLetter = "F";
        }
        else if (Input.GetKey(KeyCode.Q)) // Replace KeyCode.Q with the key you want to detect
        {
            Debug.Log("Q key is being held down!");
            selectedLetter = "Q";
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSpriteColorGreenRpc() {
        _spriteRenderer.color = Color.green;
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSpriteColorRedRpc(Vector2 targetPosition) {
        DrawLine(targetPosition);
        _spriteRenderer.color = Color.red;
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSpriteColorDefaultRpc(Vector2 targetPosition) {
        DrawLine(targetPosition);
        _spriteRenderer.color = Color.white;
    }

    private void DrawLine(Vector2 targetPosition) {
        lineRenderer.enabled = true;
        // Update the start and end points of the line
        if (lineRenderer != null) {
            // Set the start of the line at the player’s position
            lineRenderer.SetPosition(0, transform.position);
            // End of the line at the target position (convert Vector2 to Vector3 with z = 0)
            lineRenderer.SetPosition(1, new Vector3(targetPosition.x, targetPosition.y, 0));
        }
    }
    
    [Rpc(SendTo.Server)]
    void HandlePlayerClickedRpc(string selectedLetter, RpcParams rpcParams = default) {
        var clientId = rpcParams.Receive.SenderClientId;
        var objectId = NetworkObjectId;
        Debug.Log($"Client {clientId} clicked on object {objectId} owned by {OwnerClientId}");
        
        // if server says its valid action
        var targetPosition = transform.position;
        if (selectedLetter == "F")
            NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerClicked>().ChangeSpriteColorRedRpc(new Vector2(targetPosition.x, targetPosition.y));
        if (selectedLetter == "Q")
            NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerClicked>().ChangeSpriteColorDefaultRpc(new Vector2(targetPosition.x, targetPosition.y));
        
        ChangeSpriteColorGreenRpc();
    }

    private void OnMouseDown() {
        //if (!IsOwner) return;
        //_spriteRenderer.color = Color.green;
        //var clientId = NetworkManager.Singleton.LocalClientId; // the ID of the client
        //var objectId = NetworkObjectId; // The ID of the player object that was clicked

        // Send both player IDs to the server for processing
        //HandlePlayerClickedRpc(clientId, objectId);
        HandlePlayerClickedRpc(selectedLetter);
    }
}
