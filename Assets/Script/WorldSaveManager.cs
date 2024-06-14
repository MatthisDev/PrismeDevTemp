using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : MonoBehaviour
{
    [SerializeField] private int _worldSceneIndex = 0;
    public static WorldSaveManager Instance { private set; get; }
    public static SaveGameManager SaveGameManagerInstance;
    
    // Correspond au numero de la scene qu on charge
    public int WorldSceneIndex
    {
        private set => _worldSceneIndex = value;
        get => _worldSceneIndex;
    }
    
    private void Awake()
    {
        // On save que s'il n en existe pas deja une 
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
        yield return null;
    }

}
