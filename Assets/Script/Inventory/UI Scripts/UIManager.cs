using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Inventory.UI_Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { private set; get; }
        public PlayerInputManager PlayerInput => PlayerInputManager.Instance;
        
        public DynamicInventoryDisplay playerDynamicInventory;
        public DynamicInventoryDisplay playerBackpackPanel;
        public StaticInventoryDisplay playerEquipmentPanel;
        public SkillUIManager skillTreePanel;
        
        public GameObject menu;
        
        //public SaveGameManager saveGameManager;
        public Button save;
        public Button load;
        public Button delete;

        public bool IsOpenInventory { private set; get; }
        public bool IsOpenEquipment { private set; get; }
        public bool IsOpenChest { private set; get; }
        public bool IsOpenMenu {private set; get; }
        public bool IsOpenSkillTree { private set; get; }
        public bool IsPageMode { private set; get; }
        
        
        [HideInInspector] 
        public List<Action<bool>> Pages;

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value < 0) 
                    value += Pages.Count;
                _currentPage = value % Pages.Count;
            }
        }
        
        // background
        public GameObject bgInventory;
        public GameObject bgInventoryChest;
        public GameObject bgMenu;
        private void Awake() // au début tout est désactivé
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            /* par defaut tout est ferme */
            this.OpenMenu(false);
            this.OpenInventory(false);
            this.OpenSkillTree(false);
            this.OpenChestInventory(false);

            
            /* initialisation du mode page */
            this.PageInitialization(); 

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            OpenInputInventoryAction(false);
            CloseInputInventoryAction();
        }


        private void OnEnable()
        {
            //InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
            PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        }

        private void OnDisable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory; // unable
            PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        }

        void DisplayInventory(InventorySystem invToDisplay, int offset)
        {
            //playerDynamicInventory.gameObject.SetActive(true);
            playerDynamicInventory.RefreshDynamicInventory(invToDisplay, offset);
        }
        void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
        {
            playerBackpackPanel.gameObject.SetActive(true);
            playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
        }

        private void PageInitialization()
        {
            this._currentPage= 0;
            this.Pages = new List<Action<bool>>();
            
            // page 1 : Inventory - p2 Arbre de competence - p3 Menu
            this.Pages.AddRange( new Action<bool>[] {OpenInventory, OpenSkillTree, OpenMenu});
        }

        // i = 1 => page a droite si i = -1 page a gauche
        private void SwitchPage(int i)
        {
            // desactive la page actuel
            this.Pages[CurrentPage](false); 
            // passe a la page suivante
            CurrentPage += i; 
            // active la nouvelle page
            this.Pages[CurrentPage](true);
        }
        private void OpenMenu(bool active)
        {
            bgMenu.gameObject.SetActive(active);
            
            menu.gameObject.SetActive(active);
            save.gameObject.SetActive(active);
            load.gameObject.SetActive(active);
            delete.gameObject.SetActive(active);
            IsOpenMenu = active;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void OpenInventory(bool active)
        {
            if(active)
                PlayerInventoryHolder.OnPlayerInventoryDisplayRequested.Invoke(PlayerInventoryHolder.Instance.PrimaryInventorySystem, PlayerInventoryHolder.Instance.Offset);
            else
                playerBackpackPanel.gameObject.SetActive(false);
            
            // ex : Si je veux switch de page et avec un inventaire de coffre alors je le ferme aussi
            if(!active && IsOpenChest)
                this.OpenChestInventory(false);
            
            playerEquipmentPanel.gameObject.SetActive(active);
            bgInventory.gameObject.SetActive(active);
            IsOpenInventory = active;
            IsOpenEquipment = active;
        }

        private void OpenChestInventory(bool active)
        {
            if(active)
                InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
                
            bgInventoryChest.gameObject.SetActive(active);
            playerDynamicInventory.gameObject.SetActive(active);
            IsOpenChest = active;
        }

        private void OpenSkillTree(bool active)
        {
            skillTreePanel.gameObject.SetActive(active);
            IsOpenSkillTree = active;
        }

        public void OpenInputInventoryAction(bool interact)
        {
            if (!this.IsPageMode && !this.IsOpenMenu)
            {
                // si on cherche a rentrer dans le mode page
                if (!IsOpenInventory && !IsOpenEquipment && PlayerInput.inventoryInput &&
                    !playerEquipmentPanel.gameObject.activeInHierarchy)
                {
                    this.CurrentPage = 0;
                    this.IsPageMode = true;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    OpenInventory(true);
                }
                else if (PlayerInput.closeInput && !IsOpenChest)
                {
                    PlayerInput.closeInput = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    OpenMenu(true);
                }
            }else if (this.IsPageMode)
            {
                // si on est dans le mode page pour changer de page
                if (PlayerInput.pageInput != 0)
                    SwitchPage(PlayerInput.pageInput);
            }
            // si le joueur interagit avec un item alors on ouvre l'inventaire dedier
            if (interact && PlayerInput.interactInput && !IsOpenChest && !this.IsOpenMenu && (this.CurrentPage == 0))
            {
                OpenChestInventory(true);
                PlayerInput.interactInput = false;
            }
            PlayerInput.pageInput = 0;
            PlayerInput.inventoryInput = false;
        }
        public void CloseInputInventoryAction()
        {

            // Differentes actions si on appuie sur la closeInput
            if (PlayerInput.closeInput)
            {
                if (IsOpenSkillTree)
                    OpenSkillTree(false);

                if (IsOpenInventory && IsOpenEquipment)
                    // si l'inventaire (= fermeture stockage + equipement) du joueur est ouvert
                    OpenInventory(false);

                if (IsOpenChest)
                {
                    Debug.Log("desactive");
                    OpenChestInventory(false);
                }

                if(IsOpenMenu) 
                    OpenMenu(false);

                this.CurrentPage = 0;
                this.IsPageMode = false;
                PlayerInput.closeInput = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

        }


    }
}
