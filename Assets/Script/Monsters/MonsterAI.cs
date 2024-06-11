using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Unity.Mathematics;
using Random = System.Random;


public class MonsterAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerEntity PlayerEntity;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private MonsterEntity MonsterEntity;
    [SerializeField] private Animator Animator;
    [SerializeField] private bool isattacking;
    
    void Start()
    {
        setplayer();
        Agent.speed = MonsterEntity.MonsterData.speed;
    }

    
    void Update()
    {
        if (Vector3.Distance(player.position,transform.position)<MonsterEntity.MonsterData.attackradius)
        {
            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 120 * Time.deltaTime);
            if (!isattacking)
            { 
                StartCoroutine(AttackPlayer());
            } 
        }
        else {
            if (!isattacking)
            {
                Agent.SetDestination(player.position);
            }
        }
        Animator.SetFloat("Speed",Agent.velocity.magnitude);
    }

    private void setplayer()
    {
        var ran = new Random();
        var players = GameObject.FindGameObjectsWithTag("Player");
        GameObject choosenplayer = players[ran.Next(0, players.Length - 1)];
        player = choosenplayer.transform;
        PlayerEntity = choosenplayer.GetComponent<PlayerEntity>();
    }

    IEnumerator AttackPlayer()
    {
        isattacking = true;
        Agent.isStopped = true;
        Animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(MonsterEntity.MonsterData.attackdelay);
        Agent.isStopped = false;
        isattacking = false;
    }
    
}
