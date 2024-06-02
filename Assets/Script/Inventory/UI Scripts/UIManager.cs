using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Script.Inventory.UI_Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { private set; get; }
        public PlayerInputManager PlayerInput => PlayerInputManager.Instance; 
        
        public DynamicInventoryDisplay inventoryPanel;
        public DynamicInventoryDisplay playerBackpackPanel;
        public StaticInventoryDisplay playerEquipementPanel;
        public SkillUIManager skillTreePanel;
        
        public GameObject menu;
        
        //public SaveGameManager saveGameManager;
        public Button save;
        public Button load;
        public Button delete;
        
        public bool IsOpenInventory {private set; get;}
        public bool IsOpenEquipment {private set; get;}
        public bool IsOpenMenu;
        public bool IsOpenSkillTree { private set; get; } 
        public bool IsPageMode { private set; get; }

        [HideInInspector] 
        public List<Action<bool>> Pages;

        public int _currentPage;
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

        private void Awake() // au début tout est désactivé
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            skillTreePanel.gameObject.SetActive(false);
            inventoryPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
            playerEquipementPanel.gameObject.SetActive(false);

            menu.gameObject.SetActive(false);
            save.gameObject.SetActive(false);
            load.gameObject.SetActive(false);
            delete.gameObject.SetActive(false);
            
            this.PageInitialization(); // init ordre des pages
            
            
        }
        private void Update()
        {
            OpenInputInventoryAction();
            CloseInputInventoryAction();
        }

        private void OnEnable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
            PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        }

        private void OnDisable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
            PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        }

        void DisplayInventory(InventorySystem invToDisplay, int offset)
        {
            inventoryPanel.gameObject.SetActive(true);
            inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
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
            
            playerEquipementPanel.gameObject.SetActive(active);
            inventoryPanel.gameObject.SetActive(active);
            IsOpenInventory = active;
            IsOpenEquipment = active;
        }

        private void OpenSkillTree(bool active)
        {
            skillTreePanel.gameObject.SetActive(active);
            IsOpenSkillTree = active;
        }

        // implemente un systeme de page
        private void OpenInputInventoryAction()
        {
            if (!this.IsPageMode && !this.IsOpenMenu)
            {
                // si on cherche a rentrer dans le mode page
                if (!IsOpenInventory && !IsOpenEquipment && PlayerInput.inventoryInput &&
                    !playerEquipementPanel.gameObject.activeInHierarchy)
                {
                    
                    this.CurrentPage = 0;
                    this.IsPageMode = true;
                    Cursor.visible = true;
                    OpenInventory(true);
                }
                else if (PlayerInput.closeInput)
                {
                    OpenMenu(true);
                    PlayerInput.closeInput = false;
                    Cursor.visible = true;
                }
            }else if (this.IsPageMode)
            {
                // si on est dans le mode page pour changer de page
                if (PlayerInput.pageInput != 0)
                {
                    SwitchPage(PlayerInput.pageInput);
                    PlayerInput.pageInput = 0;
                }
            }
            PlayerInput.inventoryInput = false;
        }
        private void CloseInputInventoryAction()
        {

            // Differentes actions si on appuie sur la closeInput
            if (PlayerInput.closeInput)
            {
                if (IsOpenSkillTree)
                    OpenSkillTree(false);

                if (IsOpenInventory && IsOpenEquipment) // si l'inventaire (= fermeture stockage + equipement) du joueur est ouvert
                        OpenInventory(false);
                
                if(IsOpenMenu) 
                    OpenMenu(false);

                Cursor.visible = false;
                this.IsPageMode = false;
                PlayerInput.closeInput = false;
            }
        }


    }
}
