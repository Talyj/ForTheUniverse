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
        SetAttackRange(50f);
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
        GetNearestTarget();
        if(Cible != null)
        {
            var dist = Vector3.Distance(transform.position, Cible.transform.position);
            if (dist > GetAttackRange() + 5)
            {
                Cible = null;
            }
            Attack();
        }

        HealthBehaviour();

    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            Cible.GetComponent<IDamageable>().TakeDamage(GetDegMag(), DamageType.brut,photonView.ViewID);
            attackCooldown = 5;
        }
        attackCooldown -= Time.deltaTime;
    }
}
