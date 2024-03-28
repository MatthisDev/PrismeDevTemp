using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;
    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;
    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;
    protected override void Awake()
    {
        base.Awake();
        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }


    void HandleInventory()
    {
        // si l'inventaire est n'est pas encore ouvert et qu on a appuye alors on l'ouvre 
        if (!PlayerInputManager.Instance.isOpenInventory && PlayerInputManager.Instance.inventoryInput)
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
    }
    
    void Update()
    {
        var inputInstance = PlayerInputManager.Instance;
        // ouvre l'inventaire
        if (inputInstance.inventoryInput && !inputInstance.isOpenInventory)
        {
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
            inputInstance.isOpenInventory = true;
            inputInstance.inventoryInput = false;
        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data,amount))
        {
            return true;
        }
        else if (secondaryInventorySystem.AddToInventory(data,amount))
        {
            return true;
        }

        return false;
    }
}
