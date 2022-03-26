using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    public TypeDegats typeDegats;
    public float degats;
    private float vitesse = 30;
    PlayerStats stats;
    public GameObject target;
    public bool targetSet;
    protected bool stopProjectile = false;
    protected bool touched;

    public void Start()
    {
        touched = false;
    }

    private void Update()
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
                {
                    if (target.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
                    {
                        touched = true;
                        target.GetComponent<Targetable>().TakeDamage(degats, typeDegats.ToString());
                        Destroy(gameObject);
                        stopProjectile = true;

                    }
                }
            }
        }
    }
}
