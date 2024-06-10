using System;
using System.Collections;
using System.Collections.Generic;
using Script.Inventory.UI_Scripts;
using Script.Player;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Initialise le moment ou le joueur peut bouger
 * S'occupe des inputs en rapport avec des actions du joueur
 */
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { private set; get; }
    private PlayerControls PlayerControls { set; get; }
    public PlayerManager Player;

    // Input KeyBoard 
    [SerializeField] private Vector2 movementInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    public float moveAmount = 0.0f;

    // Input Camera
    [SerializeField] private Vector2 cameraInput;
    [HideInInspector] public float cameraVerticalInput;
    [HideInInspector] public float cameraHorizontalInput;

    // Input action
    [SerializeField] public bool inventoryInput;
    [SerializeField] public bool closeInput;
    [SerializeField] public int pageInput;
    [SerializeField] public bool interactInput;
    private int PageInput
    {
        get => pageInput;
        set
        {
            if (value < -1) value = -1;
            if (value > 1) value = 1;
            pageInput = value;
        }
    }

    private void Reset()
    {
        movementInput = Vector2.zero;
        verticalInput = 0;
        horizontalInput = 0;
        moveAmount = 0;

        cameraInput = Vector2.zero;
        cameraVerticalInput = 0;
        cameraHorizontalInput = 0;
        
        Player.PlayerAnimatorManager.UpdateAnimationMovementVariable(0,0);
        
    }
    // Execute avant le Start (au tout tout debut)
    private void Awake()
    {
        // On cree que s'il existe deja
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    // Une fois dans la WORLDSCENE on autorise
    private void Start()
    {
        Instance.enabled = true;
        SceneManager.activeSceneChanged += OnSceneChange;
        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        /* si on est dans la WORLDSCENE alors on run le script qui permet les mouvements
           - permet d'eviter d avoir un perso qui bouge quand on est dans une autre scene comme le menu - */
        if (newScene.buildIndex == WorldSaveManager.Instance.WorldSceneIndex)
            Instance.enabled = true;
        else
            Instance.enabled = false;
    }

    private void OnEnable()
    {
        // Si le gameObject/InputAction PlayerControls n existe pas il est cree 
        if (this.PlayerControls == null)
        {
            this.PlayerControls = new PlayerControls();
            // Update le vector2 des qu'il detecte du mouvement (Joysitck ou key)
            this.PlayerControls.PlayerMouvement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            this.PlayerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            
            this.PlayerControls.PlayerActions.Inventory.performed += i => inventoryInput = i.ReadValueAsButton();
            this.PlayerControls.PlayerActions.Close.performed += i => closeInput = i.ReadValueAsButton();
            
            // pages - if key == a or q then pageInput = -1 else if key == e then pageInput = 1 else pageInput = 0 
            this.PlayerControls.PlayerActions.RightPage.performed += i => PageInput += Convert.ToInt32(i.ReadValueAsButton());
            this.PlayerControls.PlayerActions.LeftPage.performed += i => PageInput -= Convert.ToInt32(i.ReadValueAsButton());
            
            //action
            this.PlayerControls.PlayerActions.Interact.performed += i => interactInput = i.ReadValueAsButton();
            inventoryInput = this.PlayerControls.PlayerActions.Close.IsPressed();
        }

        this.PlayerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // Si notre jeu est minimisé alors on ne detecte plus les inputs
    private void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            if (hasFocus)
                PlayerControls.Enable();
            else
            {
                PlayerControls.Disable();
            }
        }
    }
    
    private void Update()
    {
        if (UIManager.Instance != null && !UIManager.Instance.IsOpenMenu && !UIManager.Instance.IsPageMode && !UIManager.Instance.IsOpenChest)
        {
            HandleMovementInput();
            HandleCameraMovementInput();
        }
        else // on reset les input pour annuler tout mouvement durant l ouverture de l inventaire
            Reset();
    }

    private void LateUpdate()
    {
        this.interactInput = false;
        this.closeInput = false;
        this.inventoryInput = false;
        this.pageInput = 0;
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        
        // on arrondie le mouvement a une valeur sûr
        if (moveAmount <= 0.5 && moveAmount > 0)
            moveAmount = 0.5f;
        else if(moveAmount > 0.5 && moveAmount <= 1)
            moveAmount = 1;
        
        if(Player.IsSpawned) 
            // on specifie juste le mouvement avant
            Player.PlayerAnimatorManager.UpdateAnimationMovementVariable(0, moveAmount);
    }
    
    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

}
