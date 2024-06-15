using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Script.Player.Skill_System.UI
{
    public class UIStatPanel : MonoBehaviour
    {
        private Label _maxPvLabel, _strengthLabel, _defenseLabel, _intelligenceLabel;
        private Label SkillPointsLabel;

        private SkillUIManager UIManager;

        private void Awake()
        {
            UIManager = GetComponent<SkillUIManager>();
        }

        private void Start()
        {
            GatherLabelReference();
            PopulateLabelText();
        }

        private void OnEnable()
        {
            GatherLabelReference();
            PopulateLabelText();
        }

        private void GatherLabelReference()//récupère les labels
        {
            _maxPvLabel = UIManager.UiDocument.rootVisualElement.Q<Label>("StatLabel_MaxPv");
            _strengthLabel = UIManager.UiDocument.rootVisualElement.Q<Label>("StatLabel_Strength");
            _defenseLabel = UIManager.UiDocument.rootVisualElement.Q<Label>("StatLabel_Defense");
            _intelligenceLabel = UIManager.UiDocument.rootVisualElement.Q<Label>("StatLabel_Intelligence");
            SkillPointsLabel = UIManager.UiDocument.rootVisualElement.Q<Label>("SkillPointsLabel");
        }

        public void PopulateLabelText()// change le texte des labels
        {
            _maxPvLabel.text = "PV Max : "+UIManager.PlayerSkillManager.Player.maxPv.Value;
            _strengthLabel.text = "Force : "+UIManager.PlayerSkillManager.Player.strength.Value;
            _defenseLabel.text = "Defense : "+UIManager.PlayerSkillManager.Player.defense.Value;
            _intelligenceLabel.text = "Intelligence : " + UIManager.PlayerSkillManager.Player.intelligence.Value;
            SkillPointsLabel.text = "Skill Points : " + UIManager.PlayerSkillManager.Player.skillPoints;
        }
    }
}