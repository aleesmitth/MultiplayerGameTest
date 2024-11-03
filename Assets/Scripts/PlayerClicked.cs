using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerClicked : NetworkBehaviour {
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSpriteColorRpc() {
        _spriteRenderer.color = Color.green;
    }
    
    [Rpc(SendTo.Server)]
    void HandlePlayerClickedRpc(RpcParams rpcParams = default) {
        var clientId = rpcParams.Receive.SenderClientId;
        var targetObjectId = NetworkObjectId;
        Debug.Log($"Client {clientId} clicked on object {targetObjectId}");
        //ChangeSpriteColorRpc(targetObjectId);
        ChangeSpriteColorRpc();
    }

    private void OnMouseDown() {
        //if (!IsOwner) return;
        //_spriteRenderer.color = Color.green;
        //var clientId = NetworkManager.Singleton.LocalClientId; // the ID of the client
        //var objectId = NetworkObjectId; // The ID of the player object that was clicked

        // Send both player IDs to the server for processing
        //HandlePlayerClickedRpc(clientId, objectId);
        HandlePlayerClickedRpc();
    }
}
