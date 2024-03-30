using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;

    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    private void RefreshStaticDisplay()
    {
        if (inventoryHolder!= null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        
        AssignSlot(inventorySystem,0);
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }
    public override void AssignSlot(InventorySystem invToDisplay,int offset) // create dictionnary of inventoryslot_UI and their corresponding inventorySlot
    {
        slotDictionnary = new Dictionary<InventorySlot_UI, InventorySlot>();
        
        for (int i = 0; i < offset; i++)
        {
            slotDictionnary.Add(slots[i],inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
