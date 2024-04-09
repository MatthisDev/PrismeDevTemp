using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class PlayerSkillManager : MonoBehaviour
{
    public int SkillPoints;

    public PlayerEntity player;
    public List<ScriptableSkill> UnlockedSkills = new List<ScriptableSkill>();

    public void GainSkillPoint()
    {
        SkillPoints++;
    }

    public bool CanAffordSkill(ScriptableSkill skill)
    {
        return SkillPoints >= skill.Cost;
    }

    public void UnlockSkill(ScriptableSkill skill)
    {
        if (!CanAffordSkill(skill)) return;

        SkillPoints -= skill.Cost;
        player.UnlockSkill(skill);
        UnlockedSkills.Add(skill);
    }

    public bool IsSkillUnlocked(ScriptableSkill skill)
    {
        return UnlockedSkills.Contains(skill);
    }

    public bool PrereqsMet(ScriptableSkill skill)
    {
        return skill.SkillPrerequisites.Count == 0 || skill.SkillPrerequisites.All(UnlockedSkills.Contains);
    }
}
