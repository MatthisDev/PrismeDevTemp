using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[Serializable]
public class MonsterEntity : MonoBehaviour //script qui gère les stats des monstres à faire
{
    public MonsterData MonsterData;
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

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
