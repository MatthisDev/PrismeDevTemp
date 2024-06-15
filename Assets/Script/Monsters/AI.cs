using System;
using Script.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Script.Monsters
{[Serializable]
    public abstract class AI : MonoBehaviour
    {
        [SerializeField] protected Transform player;
        [SerializeField] protected PlayerManager playerManager ;
        [SerializeField] protected NavMeshAgent Agent;
        [SerializeField] protected MonsterEntity MonsterEntity;
        [SerializeField] protected Animator Animator;
        [SerializeField] protected bool isattacking;
        [SerializeField] protected bool IsDead;
        [SerializeField] protected bool canTurn;
        public abstract void die();
    }
}