using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviour : IDamageable
{
    private float damages;
    private bool isInside;
    //Modify that to the player's class
    private PlayerStats target;
    private float attackCooldown;

    public void Start()
    {
        attackCooldown = 0;
        isInside = false;
        target = null;
        SetHealth(200);
        damages = 10;
    }

    public void Update()
    {
        Attack();
        CheckDestroy();
        Debug.Log(GetHealth());
    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            if (isInside)
            {
                //Replace function by the player's one that deals damages
                target.TakeDamage(damages, "Brut");
                attackCooldown = 5;
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    public void CheckDestroy()
    {
        if (IsDead())
        {
            GameObject.Destroy(gameObject, 0);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            //Modify GameObject with the player's class
            if(target == null)
            {
                target = other.GetComponent<PlayerStats>();
            }
            isInside = true;
            Debug.Log("Inside");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            target = null;
            isInside = false;
            Debug.Log("Outside");
        }
    }
}
