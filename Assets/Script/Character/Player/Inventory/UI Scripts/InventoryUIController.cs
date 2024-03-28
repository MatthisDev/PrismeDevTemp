using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay PlayerBackpackPanel;

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        PlayerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    void Update()
    {
        var inputInstance = PlayerInputManager.Instance;
        // close l inventaire coffre
        if (inventoryPanel.gameObject.activeInHierarchy && inputInstance.isOpenInventory && (inputInstance.inventoryInput || inputInstance.closeInput))
        {
            inventoryPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
        }
        
        // close l inventaire de joueur
        if (PlayerBackpackPanel.gameObject.activeInHierarchy && inputInstance.isOpenInventory && (inputInstance.inventoryInput || inputInstance.closeInput))
        {
            PlayerBackpackPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
            inputInstance.isOpenInventory = false; 
            inputInstance.inventoryInput = false; 
            inputInstance.closeInput = false;
        }
    }

    void DisplayInventory(InventorySystem invToDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay);
    }
    void DisplayPlayerBackpack(InventorySystem invToDisplay)
    {
        PlayerBackpackPanel.gameObject.SetActive(true);
        PlayerBackpackPanel.RefreshDynamicInventory(invToDisplay);
    }
}
