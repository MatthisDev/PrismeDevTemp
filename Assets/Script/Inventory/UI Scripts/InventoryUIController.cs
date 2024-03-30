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

    void Update()
    {
        if (!inventoryPanel.gameObject.activeInHierarchy && !PlayerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Save.gameObject.activeInHierarchy)// si le menu de sauvegarde est ouvert le fermer
            {
                Save.gameObject.SetActive(false);
                Load.gameObject.SetActive(false);
                Delete.gameObject.SetActive(false);
            }
            else // sinon l'ouvrir
            {
                Save.gameObject.SetActive(true);
                Load.gameObject.SetActive(true);
                Delete.gameObject.SetActive(true);
            }
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame && !PlayerEquipementPanel.gameObject.activeInHierarchy)// ouvrir l'équipement
        {
            PlayerEquipementPanel.gameObject.SetActive(true);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && PlayerEquipementPanel.gameObject.activeInHierarchy)
        {
            PlayerEquipementPanel.gameObject.SetActive(false);
        }
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // si inventory panel est ouvert
        {
            inventoryPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
            
        }
        if (PlayerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // si l'inventaire du joueur est ouver
        {
            PlayerBackpackPanel.gameObject.SetActive(false); // desactivate inventory, swoosh disapear
        }
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
