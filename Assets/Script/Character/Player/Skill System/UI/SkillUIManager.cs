using System;
using System.Collections;
using System.Collections.Generic;
using Script.Player.Skill_System;
using Script.Player.Skill_System.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private ScriptableSkillLibrary skillLibrary;
    public ScriptableSkillLibrary SkillLibrary => skillLibrary;
    [SerializeField] private VisualTreeAsset uiTalentButton;
    public PlayerSkillManager PlayerSkillManager;
    public UIDocument UiDocument;

    private VisualElement _skillTopRow, _skillMiddleRow, _skillBottomRow;
    private List<UITalentButton> TalentButtons = new List<UITalentButton>();

    private void Awake()
    {
        UiDocument = GetComponent<UIDocument>();
        PlayerSkillManager = FindObjectOfType<PlayerSkillManager>();
    }

    private void Start()
    {
        CreateSkillButtons();
    }

    private void CreateSkillButtons()
    {
        var root = UiDocument.rootVisualElement;
        _skillTopRow = root.Q<VisualElement>("Skill_RowOne");
        _skillMiddleRow = root.Q<VisualElement>("Skill_RowTwo");
        _skillBottomRow = root.Q<VisualElement>("Skill_RowThree");
        SpawnButtons(_skillTopRow, SkillLibrary.GetSkillsOfTier(1));
        SpawnButtons(_skillMiddleRow, SkillLibrary.GetSkillsOfTier(2));
        SpawnButtons(_skillBottomRow, SkillLibrary.GetSkillsOfTier(3));
    }

    private void SpawnButtons(VisualElement parent, List<ScriptableSkill> skills)
    {
        foreach (var skill in skills)
        {
            Button clonedButton = uiTalentButton.CloneTree().Q<Button>();
            TalentButtons.Add(new UITalentButton(clonedButton, skill));
            parent.Add(clonedButton);
        }
    }
}
