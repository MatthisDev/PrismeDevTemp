using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    //public NetworkUI Instance { private set; get; }
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private GameObject Player;
    
    // Pour acceder au nb de joueur (Cette variable peut etre lu par n importe quel NetworkBehaviour)
    private NetworkVariable<int> NumberPlayer = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private void Awake()
    {
        // evenement quand on clique sur le button
        HostButton.onClick.AddListener(
            () =>
            {
                NetworkManager.Singleton.StartHost();
                Debug.Log("Start HOST");
            });
        
        ClientButton.onClick.AddListener(
            () =>
            {
                NetworkManager.Singleton.StartClient();
                Debug.Log("Start CLIENT");
                //Instantiate(Player, Vector3.zero, Quaternion.identity);
            });
        
    }
    
    /*
    public override void OnNetworkSpawn()
    {
        if(IsServer)
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
        else if (IsClient)
            RequestPlayerSpawnServerRpc();
            
    } */

    /*
    private void SpawnPlayer(ulong clientId)
    {
        GameObject playerInstance = Instantiate(Player);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        if(networkObject != null)
            networkObject.SpawnAsPlayerObject(clientId);
        else
            Debug.LogError("NetworkObject component is missing on the player prefab");
    }*/

    /*[ServerRpc(RequireOwnership = false)]
    private void RequestPlayerSpawnServerRpc(ServerRpcParams rpcParams = default)
        => SpawnPlayer(rpcParams.Receive.SenderClientId);*/
    
    public void StartNewGame()
    {
        StartCoroutine(WorldSaveManager.Instance.LoadNewGame());
    }

    /*public void Update()
    {
        if (IsHost)
        {
            foreach (var el in NetworkManager.ConnectedClientsList)
                Debug.Log(el);
            Debug.Log("----------------------------------");
        }
    }*/

}
