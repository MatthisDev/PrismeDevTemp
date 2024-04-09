using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
[Serializable]
public class PlayerStat //Class of the player stats
{
    public float BaseValue;

    public float Value {get{
        if (isDirty || BaseValue != lastBaseValue)
        {
            lastBaseValue = BaseValue;
            _value = CalculateFinalValue();
            isDirty = false;
        }

        return _value;
    }}
    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;
    
    protected readonly List<StatsModifier> statsModifiers;
    public readonly ReadOnlyCollection<StatsModifier> StatsModifiers;

    public PlayerStat()
    {
        statsModifiers = new List<StatsModifier>();
        StatsModifiers = statsModifiers.AsReadOnly();
    }
    public PlayerStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public void AddModifier(StatsModifier mod)
    {
        isDirty = true;
        statsModifiers.Add(mod);
        statsModifiers.Sort(CompareModifierOrder);
    }

    public bool RemoveModifier(StatsModifier mod)
    {

        if (statsModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;


    }

    public bool RemoveAllModifiersFromSource(object source) // supprime tout les modifiers venant d'un certain objet
    {
        bool didRemove = false;
        for (int i = statsModifiers.Count-1; i >= 0; i--)
        {
            if (statsModifiers[i].Source == source)
            {
                didRemove = true;
                isDirty = true;
                statsModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }
    protected int CompareModifierOrder(StatsModifier a, StatsModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        }
        else if (a.Order> b.Order)
        {
            return 1;
        }

        return 0;
    }
    

    protected float CalculateFinalValue() // calcule la valeur après application de tout les modifiers 
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        for (int i = 0; i < statsModifiers.Count; i++)
        {
            StatsModifier mod = statsModifiers[i];
            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd)
            {
                sumPercentAdd += mod.Value;
                if (i + 1 > statsModifiers.Count || statsModifiers[i+1].Type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);//arondir la stat final
    }
}
