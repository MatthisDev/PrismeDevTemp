using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Script.Player.Skill_System.UI
{
    public class UITalentButton
    {
        private Button _button;
        private ScriptableSkill _skill;
        private bool _isUnlocked = false;
        public static UnityAction<ScriptableSkill> OnSkillButtonClicked;
        public UITalentButton(Button assignedButton, ScriptableSkill assignedSkill)
        {
            _button = assignedButton;
            _button.clicked += OnClick;
            _skill = assignedSkill;
            if (assignedSkill.Icon) _button.style.backgroundImage = new StyleBackground(assignedSkill.Icon);
        }

        private void OnClick()
        {
            OnSkillButtonClicked?.Invoke(_skill);
        }
    }
}