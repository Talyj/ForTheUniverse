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
        //Check if target is dead
        CheckTarget();
        //Movement + attack
        DefaultMinionBehaviour();
        //Get Target in range
        GetNearestTarget();
        //Check if this gameobject is dead
        IsDead();
    }   
}
