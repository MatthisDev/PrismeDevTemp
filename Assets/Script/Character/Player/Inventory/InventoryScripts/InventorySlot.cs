using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventorySlot
{
   [SerializeField] private InventoryItemData itemData; // reference to the data
   [SerializeField] private int stackSize; // current stack size
   public InventoryItemData ItemData=> itemData;
   public int StackSize => stackSize;
   public InventorySlot(InventoryItemData source, int amount) // constructor to make a inventoy slot already occupied
   {
        itemData =source;
        stackSize = amount;
   }

   public InventorySlot() // constructor to make an empty inventory slot 
   {
       ClearSlot();
   }
   public void ClearSlot() // clears the slot
   {
       itemData = null;
       stackSize = -1;
   }

   public void AssignItem(InventorySlot invSlot) // assigns an item to the slot
   {
       if (itemData == invSlot.ItemData) AddToStack(invSlot.StackSize); // does the slot contains the same item? add to stack if so
       else
       {
           itemData = invSlot.ItemData;
           stackSize = 0;
           AddToStack(invSlot.stackSize);
       }
   }
   public void UpdateInventorySlot(InventoryItemData data, int amount) // update slot directly
   {
       itemData = data;
       stackSize = amount;
   }

   public bool RoomLeftInStack(int amountToAdd, out int amountRemaining) // would there be enouth room in the stack for the amount we're trying to add
   {
       amountRemaining = itemData.MaxStackSize - stackSize;
       return RoomLeftInStack(amountToAdd);
   }

   public bool RoomLeftInStack(int amountToAdd)
   {
       if (stackSize + amountToAdd <= itemData.MaxStackSize) return true;
       else return false;
       
   }
   public void AddToStack(int amount)
   {
       stackSize += amount;
   }
   public void RemoveFromStack(int amount)
   {
       stackSize -= amount;
   }

   public bool SplitStack(out InventorySlot splitStack)
   {
       if (stackSize <=1) // is there enougth to actually split
       {
           splitStack = null;
           return false;
       }

       int halfStack = Mathf.RoundToInt(stackSize / 2); //get half stack
       RemoveFromStack(halfStack);

       splitStack = new InventorySlot(itemData, halfStack); // create a copy slot whith half the stack
       return true;
   }
}
