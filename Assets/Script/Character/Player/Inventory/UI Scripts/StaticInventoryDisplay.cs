using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    protected override void Start()
    {
        base.Start();

        if (inventoryHolder!= null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else 
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        
        AssignSlot(inventorySystem);
    }
    public override void AssignSlot(InventorySystem invToDisplay) // create dictionnary of inventoryslot_UI and their corresponding inventorySlot
    {
        slotDictionnary = new Dictionary<InventorySlot_UI, InventorySlot>();
        
        if (slots.Length != inventorySystem.InventorySize) 
            Debug.Log($"inventory slots out of sync on {this.gameObject}");
        
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictionnary.Add(slots[i],inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
