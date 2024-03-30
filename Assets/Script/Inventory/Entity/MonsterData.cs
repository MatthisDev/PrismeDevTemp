using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject // data des monstres. en préparation
{
    public int MonsterID = -1;
    public string MonsterName;
    public Sprite MonsterSprite;
    public float PV;
    public float AttackValue;
    public InventorySystem DropsTable = new InventorySystem(5);
}
