using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Script.Monsters;
using Script.Player;
using Unity.Mathematics;
using Random = System.Random;

[Serializable]
public class MonsterAI : AI
{
    [SerializeField] private LayerMask LayerMask;
    
    void Start()
    {
        setplayer();
        Agent.speed = MonsterEntity.MonsterData.speed;
        canTurn = true;
    }

    
    void Update()
    {
        if (!IsDead)
        {
            if (!playerManager.isDead)
            {
                if (Vector3.Distance(player.position,transform.position)<MonsterEntity.MonsterData.attackradius)
                {
                    if (canTurn)
                    {
                        Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5 * Time.deltaTime);
                    }
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
            }
            else
            {
                setplayer();
            }
        
            Animator.SetFloat("Speed",Agent.velocity.magnitude);
        }
        
    }

    private void setplayer()
    {
        var ran = new Random();
        var players = GameObject.FindGameObjectsWithTag("Player");
        GameObject choosenplayer = players[ran.Next(0, players.Length - 1)];
        player = choosenplayer.transform;
        playerManager = choosenplayer.GetComponent<PlayerManager>();
    }

    IEnumerator AttackPlayer()
    {
        isattacking = true;
        Agent.isStopped = true;
        canTurn = false;
        Animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        Ray r = new Ray(transform.position + new Vector3(0,1), transform.forward);
        if (Physics.Raycast(r, out RaycastHit hit, MonsterEntity.MonsterData.attackradius,LayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerManager playerHit))
            {
                playerHit.TakeDamage(MonsterEntity.MonsterData);
            }
        }
        yield return new WaitForSeconds(MonsterEntity.MonsterData.attackdelay-1.5f);
        Agent.isStopped = false;
        isattacking = false;
    }

    public override void die()
    {
        IsDead = true;
        Agent.isStopped = true;
        Animator.SetTrigger("Death");
        
    }
    
}
