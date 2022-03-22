using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    public TypeDegats typeDegats;
    public float degats;
    public float vitesse;
    PlayerStats stats;
    public GameObject target;
    public bool stopProjectile = false;
    public bool targetSet;
    
    void Update()
    {
         
        if (target)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,vitesse * Time.deltaTime);

            if (stopProjectile == false)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 0.75f)
                {
                    if (target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
                    {
                        target.GetComponent<EnnemyStats>().TakeDamage(degats, typeDegats.ToString());
                        Destroy(gameObject);
                        stopProjectile = true;
                        
                    }
                }
            }
        }
    }
}
