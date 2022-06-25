using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{

    private IDamageable.DamageType typeDegats;
    private float degats;
    public float vitesse = 30;
    public GameObject target;
    public bool targetSet;
    protected bool stopProjectile = false;
    public bool touched;

    public void Start()
    {
        touched = false;
        //StartCoroutine(DestroyOnTime());
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
                target = null;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, vitesse * Time.deltaTime);

            if (stopProjectile == false)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 0.75f)
                    //if (touched)
                {
                    if (target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.minion ||
                       target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.voister ||
                       target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.joueur ||
                       target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.dieu ||
                       target.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.golem)
                    {
                        touched = true;
                        DealDamage(target, degats, typeDegats);
                        stopProjectile = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void SetDamages(float dmg, IDamageable.DamageType typeDmg)
    {
        degats = dmg;
        typeDegats = typeDmg;
    }

    public float GetDamages()
    {
        return degats;
    }

    public IDamageable.DamageType GetDamageType()
    {
        return typeDegats;
    }

    public void DealDamage(GameObject target, float dmg, IDamageable.DamageType typeDmg)
    {
        target.GetComponent<IDamageable>().TakeDamage(dmg, typeDmg);
    }

    public IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}
