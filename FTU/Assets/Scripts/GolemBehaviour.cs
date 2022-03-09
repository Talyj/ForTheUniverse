using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviour : IHasHealth
{
    private float damages;
    private bool isInside;
    //Modify that to the player's class
    private PlayerCompetences target;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamages(100);
        }
        Debug.Log(GetHealth());
    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            if (isInside)
            {
                //Replace function by the player's one that deals damages
                target.TakeDamages(damages);
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
                target = other.GetComponent<PlayerCompetences>();
                //target = other.gameObject;
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
