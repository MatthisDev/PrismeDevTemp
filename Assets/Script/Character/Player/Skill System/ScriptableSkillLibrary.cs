using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.Player.Skill_System
{
    [CreateAssetMenu(fileName = "New Skill Sibrary", menuName = "Skill System/New Skill Library", order = 0)]
    public class ScriptableSkillLibrary : ScriptableObject
    {
        public List<ScriptableSkill> SkillLibrary;

        public List<ScriptableSkill> GetSkillsOfTier(int tier)
        {
            return SkillLibrary.Where(skill => skill.SkillTier == tier).ToList();
        }
    }
}