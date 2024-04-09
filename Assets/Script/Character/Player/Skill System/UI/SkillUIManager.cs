using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillUIManager : MonoBehaviour
{
    public PlayerSkillManager PlayerSkillManager;
    public UIDocument UiDocument;

    private void Awake()
    {
        UiDocument = GetComponent<UIDocument>();
        PlayerSkillManager = FindObjectOfType<PlayerSkillManager>();
    }
}
