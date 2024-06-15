using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    public CharacterManager character;
    private int vertical;
    private int horizontal;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimationMovementVariable(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float h = horizontalMovement;
        float v = verticalMovement;
        if (isSprinting)
        {
            h = 2;
        }
        // on peut rajouter une precision en arrondissant au plus proche les params
        character.CharacterAnimator.SetFloat(horizontal, h,0.1f, Time.deltaTime);
        character.CharacterAnimator.SetFloat(vertical, v, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction,
        bool applyRootMotion = true)

    {
        character.CharacterAnimator.applyRootMotion = applyRootMotion;
        character.CharacterAnimator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
    }
}
