﻿using System.Collections;
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
            SetAttackRange(20f);
        }
        else SetAttackRange(10f);
        SetMoveSpeed(30f);
        SetDegMag(10f);
        SetDegPhys(10f);
        SetViewRange(30f);
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
        HealthBehaviour();
        if (Cible)
        {
            StartCoroutine(WalkToward());
            gameObject.transform.LookAt(Cible.transform);
        }        
    }

    //public void ExpFor()
    //{
    //    Instantiate(xp, transform.position, Quaternion.identity);
    //    Destroy(gameObject);
    //}
}
