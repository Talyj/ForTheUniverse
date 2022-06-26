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
        HealthBehaviour();
        CheckTarget();

        if (GetCanAct() && GetCanMove())
        {
            //Movement + attack
            DefaultMinionBehaviour();
            GetNearestTarget();
            if (Cible)
            {
                StartCoroutine(WalkToward());
                gameObject.transform.LookAt(new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z));
            }        
        }
    }

    //public void ExpFor()
    //{
    //    Instantiate(xp, transform.position, Quaternion.identity);
    //    Destroy(gameObject);
    //}
}
