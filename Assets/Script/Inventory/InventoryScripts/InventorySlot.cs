using System;
using System.Collections;
using System.Collections.Generic;
using Script.Player;
using UnityEngine;
[System.Serializable]
public class InventorySlot : ISerializationCallbackReceiver
{
   [NonSerialized] private InventoryItemData itemData; // reference to the data
   [SerializeField] private int _itemID = -1;
   [SerializeField] private int stackSize; // current stack size
   [SerializeField] public EquipementType equipementType;
   public InventoryItemData ItemData=> itemData;
   
   public int StackSize => stackSize;
   public InventorySlot(InventoryItemData source, int amount) // constructor to make a inventoy slot already occupied
   {
        itemData =source;
        _itemID = itemData.ID;
        stackSize = amount;
        if (equipementType != EquipementType.All) 
        {
            EquipItem((EquippableItem)itemData);
        }
   }

   public InventorySlot(int i)
   {
       ClearSlot();
       equipementType = (EquipementType)i;
   }
   public InventorySlot() // constructor to make an empty inventory slot 
   {
       ClearSlot();
   }
   public void ClearSlot() // clears the slot
   {
       if (equipementType != EquipementType.All)// si c'est un slot d'équipement déséquiper l'équipement
       {
           GameObject player = GameObject.FindWithTag("Player");
           PlayerManager playerManager = player.GetComponent<PlayerManager>();
           playerManager.Disarm(itemData);
       }
       itemData = null;
       _itemID = -1;
       stackSize = -1;
   }

   public void AssignItem(InventorySlot invSlot) // assigns an item to the slot
   {
       if (itemData == invSlot.ItemData) AddToStack(invSlot.StackSize); // does the slot contains the same item? add to stack if so
       else
       {
           itemData = invSlot.ItemData;
           _itemID = itemData.ID;
           stackSize = 0;
           AddToStack(invSlot.stackSize);
           if (equipementType != EquipementType.All) 
           {
               EquipItem((EquippableItem)itemData);
           }
       }
   }
   public void UpdateInventorySlot(InventoryItemData data, int amount) // update slot directly
   {
       itemData = data;
       _itemID = itemData.ID;
       stackSize = amount;
       if (equipementType != EquipementType.All)
       {
           EquipItem((EquippableItem)itemData);
       }
   }

   private void EquipItem(EquippableItem itemdata) //équipe un équipement
   {
       GameObject player = GameObject.FindWithTag("Player");
       PlayerManager playerManager = player.GetComponent<PlayerManager>();
       playerManager.Disarm(itemData);
       playerManager.EquipEquipment(itemData);
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

   public void OnBeforeSerialize()
   {
       
   }

   public void OnAfterDeserialize()
   {
       if (_itemID == -1) return;
       var db = Resources.Load<Database>("Database");
       itemData = db.GetItem(_itemID);
   }
}
