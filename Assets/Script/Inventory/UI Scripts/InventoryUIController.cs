using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay PlayerBackpackPanel;
    public StaticInventoryDisplay PlayerEquipementPanel;
    public Button Save;
    public Button Load;
    public Button Delete;

    private void Awake() // au début tout est désactivé
    {
        inventoryPanel.gameObject.SetActive(false);
        PlayerBackpackPanel.gameObject.SetActive(false);
        PlayerEquipementPanel.gameObject.SetActive(false);
        Save.gameObject.SetActive(false);
        Load.gameObject.SetActive(false);
        Delete.gameObject.SetActive(false);
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

    private void OpenMenu(bool active)
    {
        Save.gameObject.SetActive(active);
        Load.gameObject.SetActive(active);
        Delete.gameObject.SetActive(active);
    }
    private void CloseInputInventoryAction()
    {
        var inputInstance = PlayerInputManager.Instance;
        if (inputInstance.isOpenInventory && !inputInstance.isOpenEquipment &&!PlayerEquipementPanel.gameObject.activeInHierarchy)
        { 
            PlayerEquipementPanel.gameObject.SetActive(true);
            inputInstance.isOpenEquipment = true;
        }
        
        // Differentes actions si on appuie sur la closeInput
        if (inputInstance.closeInput)
        {
            if (!inventoryPanel.gameObject.activeInHierarchy && !inputInstance.isOpenInventory)
            {
                if(Save.gameObject.activeInHierarchy)
                    OpenMenu(false);
                else
                    OpenMenu(true);
            }
            
            if (inventoryPanel.gameObject.activeInHierarchy) // si inventory panel est ouvert
            {
                inventoryPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
                
            }

            if (inputInstance.isOpenInventory) // si l'inventaire du joueur est ouver
            {
                PlayerBackpackPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
                inputInstance.isOpenInventory = false;
            }

            if (PlayerEquipementPanel.gameObject.activeInHierarchy) // on close l'equipement UI
            {
                PlayerEquipementPanel.gameObject.SetActive(false);
                inputInstance.isOpenEquipment = false;
            }

            inputInstance.closeInput = false;
        }
    }

    private void Update()
    {
        CloseInputInventoryAction();
    }

    void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
    void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        PlayerBackpackPanel.gameObject.SetActive(true);
        PlayerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
