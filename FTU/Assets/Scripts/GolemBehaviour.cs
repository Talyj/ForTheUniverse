using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviour : MonoBehaviour
{
    private float health;
    private float damages;
    private bool isInside;
    //Modify that to the player's class
    private GameObject target;
    private float attackCooldown;


    public void Start()
    {
        attackCooldown = 0;
        health = 50000;
        isInside = false;
        target = null;
    }

    public void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {
            if (isInside)
            {
                //Replace function by the player's one that deals damages
                //target.ReceiveDamages(damages);
                target.SetActive(false);
                attackCooldown = 5;
            }
        }
        attackCooldown -= Time.deltaTime;
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
    }

    public void CheckDestroy()
    {
        if (health <= 0)
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
                //target = other.GetComponent<GameObject>();
                target = other.gameObject;
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
