using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipementType
{
    All,
    Helmet,
    Chest,
    Gloves,
    Boots,
    Weapon,
    Accessory1,
    Accessory2,
}
[Serializable]
[CreateAssetMenu(menuName = "Inventory System/Equippable Item")]
public class EquippableItem : InventoryItemData // classe des équipements
{   //bonus plat
    public float StrengthBonus;
    public float PvBonus;
    public float DefenseBonus;
    public float IntelligenceBonus;
    [Space] // pour 10% d'amélioration il faut mettre 0.1
    public float StrengthPercentBonus;
    public float PvPercentBonus;
    public float DefensePercentBonus;
    public float IntelligencePercentBonus;
    [Space] 
    public EquipementType EquipementType;
}
