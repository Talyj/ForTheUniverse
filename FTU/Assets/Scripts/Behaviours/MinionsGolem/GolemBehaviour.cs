using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviour : BasicAIStats
{
    private float attackCooldown;

    public void Start()
    {
        BaseInit();
        AISetup();
        SetHealth(2500f);
        SetMaxHealth(2500f);
        SetAttackRange(30f);
        SetViewRange(GetAttackRange());
        SetAttackSpeed(2.0f);
        SetDegMag(200);
        SetDegPhys(200);
        SetEnemyType(EnemyType.golem);
        attackCooldown = 0;
        Cible = null;
    }

    public void Update()
    {

        if (Cible == null)
        {
            GetNearestTarget();
        }
        else
        {
            Attack();
            var dist = Vector3.Distance(transform.position, Cible.transform.position);
            if (dist > GetAttackRange() + 5)
            {
                Cible = null;
            }
        }
        HealthBehaviour();

    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            Cible.GetComponent<IDamageable>().TakeDamage(GetDegMag(), DamageType.brut);
            attackCooldown = 5;
        }
        attackCooldown -= Time.deltaTime;
    }
}
