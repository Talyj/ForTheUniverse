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
            SetAttackRange(30f);
        }
        else SetAttackRange(10f);
        isAttacking = false;        
    }

    public void Update()
    {
        //Check if target is dead
        CheckTarget();
        //Movement + attack
        DefaultMinionBehaviour();
        //Get Target in range
        GetNearestTarget();
        //Check if this gameobject is dead
        IsDead();
        if(Cible)
        {
            WalkToTarget();
        }   
        //if (IsDead() == true)
        //{
        //    ExpFor();
        //}
    }

    //public void ExpFor()
    //{
    //    Instantiate(xp, transform.position, Quaternion.identity);
    //    Destroy(gameObject);
    //}
}
