using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Player;
using UnityEngine;
[Serializable]
public class PlayerSkillManager : MonoBehaviour
{

    public List<ScriptableSkill> UnlockedSkills = new List<ScriptableSkill>();
    public PlayerManager Player { private set; get; }
    public void Awake()
    {
        Player = GetComponent<PlayerManager>();
    }

    public void GainSkillPoint()
        => Player.skillPoints++;

    public bool CanAffordSkill(ScriptableSkill skill)
        => Player.skillPoints >= skill.Cost;

    public void UnlockSkill(ScriptableSkill skill)
    {
        if (!CanAffordSkill(skill)) return;

        Player.skillPoints -= skill.Cost;
        Player.UnlockSkill(skill);
        UnlockedSkills.Add(skill);
    }

    public bool IsSkillUnlocked(ScriptableSkill skill)
         => UnlockedSkills.Contains(skill);

    public bool PrereqsMet(ScriptableSkill skill)
         => skill.SkillPrerequisites.Count == 0 || skill.SkillPrerequisites.All(UnlockedSkills.Contains);
}
