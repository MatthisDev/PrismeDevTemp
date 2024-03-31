using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder // inventaire du joueur
{
    
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
    
    [SerializeField]public GameObject InventoryInterface;
    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.playerInventory.invSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    void Update()
    {
        PlayerInputManager inputInstance = PlayerInputManager.Instance;
        if (!inputInstance.isOpenInventory && inputInstance.inventoryInput) // si on peut ouvrir l'inventaire (touche + pas deja ouvert)
        {
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset); // ouvre l'inventaire du joueur
            inputInstance.isOpenInventory = true;
        }
        inputInstance.inventoryInput = false;
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data,amount))
        {
            return true;
        }
        return false;
    }
}
