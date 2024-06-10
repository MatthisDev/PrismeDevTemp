using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Script.Player.Skill_System.UI
{
    public class UISkillDescriptionPanel : MonoBehaviour
    {
        private SkillUIManager UIManager;
        private ScriptableSkill AssignedSkill;
        private VisualElement _skillImage;
        private Label _skillNameLabel, _skillDescriptionLabel, _skillCostLabel, _skillPreReqLabel;
        private Button purchaseSkillButton;

        private void Awake()
        {
            UIManager = GetComponent<SkillUIManager>();
        }

        private void OnEnable()
        {
            UITalentButton.OnSkillButtonClicked += PopulateLabelText;
            UIManager = GetComponent<SkillUIManager>();
            GatherLabelReferences();
            var skill = UIManager.SkillLibrary.GetSkillsOfTier(1)[0];
            PopulateLabelText(skill);
        }

        private void OnDisable()
        {
            UITalentButton.OnSkillButtonClicked -= PopulateLabelText;
            if (purchaseSkillButton != null) purchaseSkillButton.clicked -= purchaseSkill;
        }

        private void Start()
        {
            UIManager = GetComponent<SkillUIManager>();
            GatherLabelReferences();
            
        }

        private void GatherLabelReferences()
        {
            _skillImage = UIManager.UiDocument.rootVisualElement.Q<VisualElement>("icon");
            _skillNameLabel= UIManager.UiDocument.rootVisualElement.Q<Label>("SkillNameLabel");
            _skillDescriptionLabel= UIManager.UiDocument.rootVisualElement.Q<Label>("SkillDescriptionLabel");
            _skillCostLabel= UIManager.UiDocument.rootVisualElement.Q<Label>("SkillCostLabel");
            _skillPreReqLabel= UIManager.UiDocument.rootVisualElement.Q<Label>("PreReqLabel");
            purchaseSkillButton= UIManager.UiDocument.rootVisualElement.Q<Button>("SkillBuyButton");
            purchaseSkillButton.clicked += purchaseSkill;
        }

        private void purchaseSkill()
        {
            if (UIManager.PlayerSkillManager.CanAffordSkill(AssignedSkill))
            {
                UIManager.PlayerSkillManager.UnlockSkill(AssignedSkill);
                PopulateLabelText(AssignedSkill);
                UIManager.uIStatPanel.PopulateLabelText();
            }
        }

        private void PopulateLabelText(ScriptableSkill skill)
        {
            if (skill is null) return;
            AssignedSkill = skill;
            if (AssignedSkill.Icon) _skillImage.style.backgroundImage = new StyleBackground(AssignedSkill.Icon);
            _skillNameLabel.text = AssignedSkill.SkillName;
            _skillDescriptionLabel.text = AssignedSkill.SkillDescription;
            _skillCostLabel.text = $"Cout: {skill.Cost}";
            if (AssignedSkill.SkillPrerequisites.Count>0)
            {
                _skillPreReqLabel.text = "Prérequis : ";
                foreach (var prereq in skill.SkillPrerequisites)
                {
                    string punctuation =
                        prereq == AssignedSkill.SkillPrerequisites[AssignedSkill.SkillPrerequisites.Count - 1] ? "" : ",";
                    _skillPreReqLabel.text += $"{prereq.SkillName}{punctuation}";
                }
            }
            else
            {
                _skillPreReqLabel.text = "";
            }

            if (UIManager.PlayerSkillManager.IsSkillUnlocked(AssignedSkill))
            {
                purchaseSkillButton.text = "Acheté";
                purchaseSkillButton.SetEnabled(false);
            }
            else if (!UIManager.PlayerSkillManager.PrereqsMet(AssignedSkill))
            {
                purchaseSkillButton.text = "Prérequis non débloqués";
                purchaseSkillButton.SetEnabled(false);
            }
            else if (!UIManager.PlayerSkillManager.CanAffordSkill(AssignedSkill))
            {
                purchaseSkillButton.text = "trop cher";
                purchaseSkillButton.SetEnabled(false);
            }
            else
            {
                purchaseSkillButton.text = "Acheter";
                purchaseSkillButton.SetEnabled(true);
            }
        }
    }
}