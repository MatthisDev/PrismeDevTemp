using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder // inventaire du joueur
{
    public static PlayerInventoryHolder Instance { get; private set; }
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
    
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

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /*void Update()
    {
        PlayerInputManager inputInstance = PlayerInputManager.Instance;
        if (!inputInstance.isOpenInventory && inputInstance.inventoryInput) // si on peut ouvrir l'inventaire (touche + pas deja ouvert)
        {
            InventorySystem test = new InventorySystem(offset);
            
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset); // ouvre l'inventaire du joueur
            inputInstance.isOpenInventory = true;
            Cursor.visible = true; // lorsque l'inventaire est ouvert on active le curseur
        }
        inputInstance.inventoryInput = false;
    }*/

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data,amount))
        {
            return true;
        }
        return false;
    }
}
