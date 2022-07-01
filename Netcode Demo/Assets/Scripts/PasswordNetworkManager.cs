using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Text;
public class PasswordNetworkManager : MonoBehaviour
{
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private GameObject passwordEntryUI;
    [SerializeField] private GameObject leaveButton;

    // two gameobjects; one for the host and one for the client

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }
    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return; }
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }
    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        //set host to the created gameobject

        // add "host" ui element on host gameobject when host appears

    }
    public void Client()
    {
        // add a new method to verify same host and host material (if else checks if possible)
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();
        //set client to created gameobject
    }
    public void Leave()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
        passwordEntryUI.SetActive(true);
        leaveButton.SetActive(false);
    } 
    private void ApprovalCheck(byte[] connectionData, ulong ClientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string password = Encoding.ASCII.GetString(connectionData);
        bool approveConnection = password == passwordInputField.text;
        Vector3 spawnPos = Vector3.zero;
        // Quaternion spawnRot = Quaternion.identity;
        if(NetworkManager.Singleton.IsHost)
        {
            spawnPos = new Vector3(5.2f, 4.76f, 5.1f);
            // spawnRot = spawnRot = new Quaternion(0f, 0f, 0f, 0f);
        }
        switch(NetworkManager.Singleton.ConnectedClients.Count)
        {
            case 1: spawnPos = new Vector3(1f, 4.76f, 5.1f);
                    // spawnRot = new Quaternion(0f, 180f, 0f, 0f);
                    break;

            case 2: spawnPos = new Vector3(9f, 4.76f, 5.1f);
                    // spawnRot = new Quaternion(0f, 225f, 0f, 0f);
                    break;

        }
        callback(true, null, approveConnection, spawnPos, null);
        spawnPos = Vector3.zero;
        // spawnRot = Quaternion.identity;
    }
    private void HandleClientConnected(ulong ClientId)
    {
        if (ClientId == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(false);
            leaveButton.SetActive(true);
        }
    }
    private void HandleClientDisconnect(ulong ClientId)
    {
        if (ClientId == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }
    }
    private void HandleServerStarted()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }
}