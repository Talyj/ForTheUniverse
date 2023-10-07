using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GolemBehaviour : BasicAIStats
{
    private float attackCooldown;
    [SerializeField] public bool isInvincible;
    [SerializeField] private GameObject otherGolem;

    [SerializeField] private VisualEffect attack_animation;
    [SerializeField] private VisualEffect energy_sphere;
    private CapsuleCollider col;

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
        col = GetComponent<CapsuleCollider>();
        col.enabled = !isInvincible;
    }

    public void Update()
    { 
        //if (GetHealth() <= (0.2f*GetMaxHealth())) {
        //    energy_sphere.SetFloat("SpawnRate", 50.0f);
        //} else if (GetHealth() <= (0.4f*GetMaxHealth())) {
        //    energy_sphere.SetFloat("SpawnRate", 500.0f);
        //} else if (GetHealth() <= (0.6f*GetMaxHealth())) {
        //    energy_sphere.SetFloat("SpawnRate", 5000.0f);
        //} else if (GetHealth() <= (0.8f*GetMaxHealth())) {
        //    energy_sphere.SetFloat("SpawnRate", 50000.0f);
        //} 

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
        if (!isInvincible)
        {
            col.enabled = !isInvincible;
        }
        else
        {
            CheckFirstGolem();
        }

    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            //VisualEffect vfx = Instantiate(attack_animation, Cible.transform.position, Quaternion.identity);
            //vfx.Play();
            Cible.GetComponent<IDamageable>().TakeDamage(GetDegMag(), DamageType.brut,photonView.ViewID);
            attackCooldown = 5;
        }
        attackCooldown -= Time.deltaTime;
    }

    private IEnumerator DestroyAttackVFX(VisualEffect vfx, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(vfx);
    }

    private void CheckFirstGolem()
    {
        if(otherGolem == null)
        {
            isInvincible = false;
        }
    }
}
