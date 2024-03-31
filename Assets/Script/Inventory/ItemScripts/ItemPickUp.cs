using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour// script des objets ramassable
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;
    [SerializeField] private float _rotationspeed = 20f;
    
    private SphereCollider myCollider;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;
    
    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);
        
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationspeed * Time.deltaTime);
    }

    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;
        if (inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _rotation;
    }
}