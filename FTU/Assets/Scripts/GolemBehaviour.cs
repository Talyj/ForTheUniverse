using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviour : IDamageable
{
    private bool isInside;
    //Modify that to the player's class
    private float attackCooldown;

    public void Start()
    {
        attackCooldown = 0;
        isInside = false;
        Cible = null;
    }

    public void Update()
    {
        Attack();
        IsDead();
    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            if (isInside)
            {
                //Replace function by the player's one that deals damages
                Cible.GetComponent<IDamageable>().TakeDamage(DegatsMagique, "Brut");
                attackCooldown = 5;
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("minion") || other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<IDamageable>().team != team)
            {
                //Modify GameObject with the player's class
                if(Cible == null)
                {
                    Cible = other.gameObject;
                }
                isInside = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("minion") || other.gameObject.CompareTag("Player"))
        {
            Cible = null;
            isInside = false;
        }
    }
}
