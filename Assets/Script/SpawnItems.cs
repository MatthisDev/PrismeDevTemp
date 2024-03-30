using System;
using System.Collections;
using System.Collections.Generic;
using Script.Player;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class SpawnItems : MonoBehaviour
{
    public static SpawnItems Instance { private set; get; }

    // Spawn quand le HOTE spawn -> sinon bug
    public GameObject InventairePrefabs;
    public GameObject ItemsPrefabs;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Spawn()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            Instantiate(InventairePrefabs, Vector3.zero, Quaternion.identity);
            Instantiate(ItemsPrefabs, Vector3.zero, Quaternion.identity);
        }
    }
}
