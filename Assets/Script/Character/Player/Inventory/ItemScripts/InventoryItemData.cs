using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this is a scritpable object, that define what an item is;
/// it can be inherited from to have branched version of items, like potions or weapons
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    [TextArea(4,4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
}
