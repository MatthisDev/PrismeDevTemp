using System;
using System.Collections;
using System.Collections.Generic;
using Script.Monsters;
using Script.Player;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;


[Serializable]
public class BossAI : AI
{
    private int attackType;

    public LayerMask LayerMask;
    // Start is called before the first frame update
    void Start()
    {
        setplayer();
        Agent.speed = MonsterEntity.MonsterData.speed;
        canTurn = true;
    }

    // Update is called once per frame
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
            setplayer();
            Animator.SetFloat("Speed",Agent.velocity.magnitude);
        }
    }

    private void setplayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        GameObject choosenplayer = players[0];
        foreach (var p in players)
        {
            if (Vector3.Distance(p.transform.position,transform.position)<(Vector3.Distance(choosenplayer.transform.position,transform.position)))
            {
                choosenplayer = p;
            }
        }
        player = choosenplayer.transform;
        playerManager = choosenplayer.GetComponent<PlayerManager>();
    }

    public override void die()
    {
        IsDead = true;
        Agent.isStopped = true;
        Animator.SetTrigger("Death");
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
        canTurn = true;
        Agent.isStopped = false;
        isattacking = false;
    }

    private void attackDecision()
    {
        Animator.SetInteger("AttackType",attackType);
        if (attackType == 0)
        {
            attackType = 1;
        }
        else if (attackType == 1)
        {
            attackType = 2;
        }
        else
        {
            attackType = 0;
        }
        Animator.SetTrigger("Attack");
    }
}
