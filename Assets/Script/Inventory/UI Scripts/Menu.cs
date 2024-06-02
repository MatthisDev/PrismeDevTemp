using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour //class pour gérer le futur menu échap
{
    //public SaveGameManager SaveGameManager;
    public Button Save;
    public Button Load;
    public Button Delete;

    private void Awake()
    {
        Save.gameObject.SetActive(true);
        Load.gameObject.SetActive(true);
        Delete.gameObject.SetActive(true);
    }
}
