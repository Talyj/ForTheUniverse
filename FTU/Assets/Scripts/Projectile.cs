using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    public TypeDegats typeDegats;
    public float degats;
    public float vitesse = 30;
    PlayerStats stats;
    public GameObject target;
    public bool targetSet;
    protected bool stopProjectile = false;
    public bool touched;

    public void Start()
    {
        touched = false;
    }

    public void Update()
    {

        Behaviour();
    }

    public void Behaviour()
    {
        if (target)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, vitesse * Time.deltaTime);

            if (stopProjectile == false)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 0.75f)
                    //if (touched)
                {
                    if (target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion ||
                       target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.voister ||
                       target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.joueur ||
                       target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.dieu ||
                       target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.golem)
                    {
                        touched = true;
                        DealDamage(target, degats, typeDegats.ToString());
                        stopProjectile = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void DealDamage(GameObject target, float dmg, string typeDmg)
    {
        target.GetComponent<Targetable>().TakeDamage(dmg, typeDmg);
    }

    //    public void OnTriggerEnter(Collider other)
    //    {
    //        if(other.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion ||
    //        other.GetComponent<Targetable>().enemytype == Targetable.EnemyType.voister ||
    //        other.GetComponent<Targetable>().enemytype == Targetable.EnemyType.joueur ||
    //        other.GetComponent<Targetable>().enemytype == Targetable.EnemyType.dieu ||
    //        other.GetComponent<Targetable>().enemytype == Targetable.EnemyType.golem)
    //        {
    //            touched = true;
    //            DealDamage(target, degats, typeDegats.ToString());
    //            stopProjectile = true;
    //            Destroy(gameObject);
    //        }
    //    }
}
