using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionnary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionnary => slotDictionnary;

    protected virtual void Start()
    {
        
    }

    public virtual void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        
    }

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionnary)
        {
            if (slot.Value == updatedSlot) // slot value - the "under the hood " inventory slot
            {
                slot.Key.UpdateUISlot(updatedSlot); //slot key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot) //TO DO case for the equipement
    {
        InventoryItemData itemData = mouseInventoryItem.AssignedInventorySlot.ItemData;
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        
        // clicked slot has an item - mouse doesn't have an item  - pickup that item
        
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && itemData == null)
        {
            // if player holding shift key? Split the stack
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot)) // split stack
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }
        
        // clicked slot doesn't have an item - mouse does have an item - place the mouse item into the empty slot
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && itemData != null) 
        {
            if (clickedUISlot.AssignedInventorySlot.equipementType != EquipementType.All) // si c'est un Equipement slot  vérifier si l'item est du bon type{
            {
                if (itemData is not EquippableItem) return;
                    
                if (clickedUISlot.AssignedInventorySlot.equipementType != ((EquippableItem)itemData).EquipementType) return; 
            }
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();
            mouseInventoryItem.ClearSlot();
            return;
        }
        //both slots have an item - decide what to do...
            
                // if the slot stack size + mouse stack size > the slot max stack size? If soo take from mouse.
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && itemData != null)
        {
            
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == itemData;
            // are both items the same? if so combine them.
            if  ( isSameItem && clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            else if (isSameItem && !clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize,out int leftInStack))
            {
                if (leftInStack<1) SwapSlots(clickedUISlot); // stack is full so swap
                else // slot is not at max, so take what's needed from the mouse inventory
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();
                    var newItem = new InventorySlot(itemData, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            
            //If different items, then swap the items.
            else if (!isSameItem)
            {
                if (clickedUISlot.AssignedInventorySlot.equipementType != EquipementType.All) // si c'est un Equipement slot  vérifier si l'item est du bon type{
                {
                    if (itemData is not EquippableItem) return;
                    
                    if (clickedUISlot.AssignedInventorySlot.equipementType != ((EquippableItem)itemData).EquipementType) return; 
                }
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot) // la fonction qui échange les slots
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}