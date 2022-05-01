using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissoinProjBehaviour : Projectile
{
    public MermaidBehaviour source;
    [SerializeField] private GameObject puddle;

    public new void Start()
    {
        StartCoroutine(SpawnPuddle());
        touched = false;
    }

    public new void Update()
    {
        if(touched == true)
        {
            source.AddPassive();            
        }
        Behaviour();        
    }

    private new void Behaviour()
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
                    if (target.GetComponent<IDamageable>().enemyType == IDamageable.EnemyType.minion ||
                       target.GetComponent<IDamageable>().enemyType == IDamageable.EnemyType.voister ||
                       target.GetComponent<IDamageable>().enemyType == IDamageable.EnemyType.joueur ||
                       target.GetComponent<IDamageable>().enemyType == IDamageable.EnemyType.dieu ||
                       target.GetComponent<IDamageable>().enemyType == IDamageable.EnemyType.golem)
                    {
                        Instantiate(puddle, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
                        touched = true;
                        DealDamage(target, degats, typeDegats.ToString());
                        stopProjectile = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private IEnumerator SpawnPuddle()
    {
        yield return new WaitForSeconds(1);
        Instantiate(puddle, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        Destroy(gameObject, 1);
    }
}
