using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoneTriggerSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> monstersPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var pre in monstersPrefab)
            {
                Instantiate(pre, this.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
