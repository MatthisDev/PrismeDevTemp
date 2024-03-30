using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable // inventaire des coffres
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var chestSavedData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSavedData);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID,out InventorySaveData chestdata))
        {
            this.primaryInventorySystem = chestdata.invSystem;
            this.transform.position = chestdata.position;
            this.transform.rotation = chestdata.rotation;
        }
    }

    public void Interact(Interactor interactor, out bool InteractSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem,0); // affiche l'inventaire du coffre
        InteractSuccessful = true;
    }

    public void EndInteraction()
    {
        
    }
}

