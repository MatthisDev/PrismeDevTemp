using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    private CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimationMovementVariable(float horizontal, float vertical)
    {
        // on peut rajouter une precision en arrondissant au plus proche les params
        character.CharacterAnimator.SetFloat("Horizontal", horizontal,0.1f, Time.deltaTime);
        character.CharacterAnimator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
    }
}
