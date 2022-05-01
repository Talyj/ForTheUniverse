using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsBehaviour : PlayerStats
{
    public void Start()
    {
        Init();
        current = 0;
        pathDone = false;
        if (attackType == AttackType.Ranged)
        {
            AttackRange = 30f;
        }
        else AttackRange = 10f;
        isAttacking = false;        
    }

    public void Update()
    {
        DefaultMinionBehaviour();        
    }   
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IDamageable>().team != team)
        {
            if(other.gameObject.CompareTag("minion") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("golem"))
            {
                Cible = other.gameObject;
            }
        }
    }
}
