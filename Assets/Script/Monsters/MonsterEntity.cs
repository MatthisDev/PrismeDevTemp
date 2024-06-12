using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Script.Monsters;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MonsterEntity : MonoBehaviour //script qui gère les stats des monstres à faire
{
    public MonsterData MonsterData;
    public AI MonsterAI;
    public float Pv;

    private void Awake()
    {
        Pv = MonsterData.PV;
    }

    
    public void TakeDamage(float Damage)
    {
        Pv -= Damage;
        if (Pv<=0)
        {
            Death();
        }
    }

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Death();
        }
    }

    private void Death()
    {
        MonsterAI.die();
    }
}
