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
            });
        
    }
    public void StartNewGame()
    {
        StartCoroutine(WorldSaveManager.Instance.LoadNewGame());
    }


    private void Update()
    {
    }
}
