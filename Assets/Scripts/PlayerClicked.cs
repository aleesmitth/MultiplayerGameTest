using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerClicked : NetworkBehaviour {
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private string selectedLetter = "F";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    private void ChangeSpriteColorRedRpc() {
        _spriteRenderer.color = Color.red;
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSpriteColorDefaultRpc() {
        _spriteRenderer.color = Color.white;
    }
    
    [Rpc(SendTo.Server)]
    void HandlePlayerClickedRpc(string selectedLetter, RpcParams rpcParams = default) {
        var clientId = rpcParams.Receive.SenderClientId;
        var objectId = NetworkObjectId;
        Debug.Log($"Client {clientId} clicked on object {objectId} owned by {OwnerClientId}");
        
        // if server says its valid action
        ChangeSpriteColorGreenRpc();
        
        if (selectedLetter == "F")
            NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerClicked>().ChangeSpriteColorRedRpc();
        if (selectedLetter == "Q")
            NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerClicked>().ChangeSpriteColorDefaultRpc();
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
