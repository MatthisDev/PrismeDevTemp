using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// script qui gère les stats du joueur
public class PlayerEntity : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    
    public PlayerStat MaxPv;
    public PlayerStat Strength;
    public PlayerStat Defense;
    public PlayerStat Intelligence;

    public float PV;

    public float Exp;
    public float NecessaryExp;
    public int Level;
    public int SkillPoints;

    public bool IsDead;

    private void Awake()
    {
        NecessaryExp = 100;
        Level = 1;
        PV = MaxPv.Value;
    }
    public void EquipEquipement(InventoryItemData item) // Ajouter tous les bonus possibles venant de l'équipement
    {
        bool PvAtmMax = MaxPv.Value == PV;
        EquippableItem equipement = (EquippableItem)item;
        MaxPv.AddModifier(new StatsModifier(equipement.PvBonus,StatModType.Flat,item));
        MaxPv.AddModifier(new StatsModifier(equipement.PvPercentBonus,StatModType.PercentMult,item));
        Strength.AddModifier(new StatsModifier(equipement.StrengthBonus,StatModType.Flat,item));
        Strength.AddModifier(new StatsModifier(equipement.StrengthPercentBonus,StatModType.PercentMult,item));
        Defense.AddModifier(new StatsModifier(equipement.DefenseBonus,StatModType.Flat,item));
        Defense.AddModifier(new StatsModifier(equipement.DefensePercentBonus,StatModType.PercentMult,item));
        Intelligence.AddModifier(new StatsModifier(equipement.IntelligenceBonus,StatModType.Flat,item));
        Intelligence.AddModifier(new StatsModifier(equipement.IntelligencePercentBonus,StatModType.PercentMult,item));
        Debug.Log("equiped");
        if (PvAtmMax)
        {
            PV = MaxPv.Value;
        }
        
    }

    public void DesequipEquipement(InventoryItemData item) // enlever tous les bonus octroyé par l'équipement
    {
        MaxPv.RemoveAllModifiersFromSource(item);
        Strength.RemoveAllModifiersFromSource(item);
        Defense.RemoveAllModifiersFromSource(item);
        Intelligence.RemoveAllModifiersFromSource(item);
        if (PV > MaxPv.Value) // mettre à jour les pv si ils sont décendu
        {
            PV = MaxPv.Value;
        }
    }

    public void UnlockSkill(ScriptableSkill skill)
    {
        foreach (UpgradeData upgradeData in skill.UpgradeDatas)
        {
            if (upgradeData.StatType == StatType.PvMax)
            {
                MaxPv.AddModifier(upgradeData.Modifier);
            }
            else if (upgradeData.StatType == StatType.Force)
            {
                Strength.AddModifier(upgradeData.Modifier);
            }
            else if (upgradeData.StatType == StatType.Defense)
            {
                Defense.AddModifier(upgradeData.Modifier);
            }
            else if (upgradeData.StatType == StatType.Intelligence)
            {
                Intelligence.AddModifier(upgradeData.Modifier);
            }
        }
    }

    public void TakeDamage(MonsterData monsterData)
    {
        PV -= monsterData.AttackValue * (1 - Defense.Value / 100);
        PV = (float)Math.Round(PV,2);
        if (PV<=0 && !IsDead)
        {
            Die();
        }
    }
    

    private void Die()
    {
        IsDead = true;
        Animator.SetTrigger("Die");
    }

    public void GainExp(float xp)
    {
        Exp += xp;
        while (Exp>= NecessaryExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Exp -= NecessaryExp;
        Level += 1;
        SkillPoints += 1;
        NecessaryExp *= 1.2f;
    }
}
