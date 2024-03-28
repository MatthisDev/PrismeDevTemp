using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TitleScreenManagers : MonoBehaviour
{
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        StartCoroutine(WorldSaveManager.Instance.LoadNewGame());
    }
}
