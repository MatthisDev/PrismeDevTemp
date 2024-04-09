using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill System/New Skill", order = 0)] 
public class ScriptableSkill : ScriptableObject// classe des compétences 
{
    public int ID = -1;
    public string SkillName;
    public List<UpgradeData> UpgradeDatas = new List<UpgradeData>();
    public bool IsAbility = false;
    public bool OverrideDescription;
    [TextArea(1, 4)] public string SkillDescription;
    public Sprite Icon;
    public List<ScriptableSkill> SkillPrerequisites = new List<ScriptableSkill>();
    public int SkillTier;
    public int Cost;

    public void OnValidate()
    {
        if (SkillName == "") SkillName = name;
        if (UpgradeDatas.Count == 0)return;
        if (OverrideDescription)return;
        GenerateDescription();
    }

    private void GenerateDescription()// génère la description en fonction des modificateurs
    {
        if (IsAbility)
        {
            // à implémenter quand on aura des compétences à débloquer
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{SkillName} augmente ");
            for (int i = 0; i < UpgradeDatas.Count; i++)
            {
                sb.Append(UpgradeDatas[i].StatType.ToString());
                sb.Append(" de ");
                if (UpgradeDatas[i].Modifier.Type == StatModType.Flat)
                {
                    sb.Append($"{UpgradeDatas[i].Modifier.Value} points ");
                }
                else
                {
                    sb.Append($"{UpgradeDatas[i].Modifier.Value * 100}%");
                }

                sb.Append(i < UpgradeDatas.Count ? "," : ".");
            }

            SkillDescription = sb.ToString();
        }
        
    }
}
[Serializable]
public class UpgradeData
{
    public StatsModifier Modifier;
    public StatType StatType;
}

public enum StatType
{
    PvMax,
    Force,
    Defense,
    Intelligence,
}

