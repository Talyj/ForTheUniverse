using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddlePoissoinBehaviour : Projectile
{
    private int insideArea;
    private List<GameObject> targets = new List<GameObject>();
    public IDamageable.Team team;
    private float cpt;

    // Start is called before the first frame update
    public new void Start()
    {
        insideArea = 0;
        cpt = 0.5f;
    }
    // Update is called once per frame
    public new void Update()
    {
        //Behaviour();
        cpt -= Time.deltaTime;
        if (cpt < 0)
        {
            cpt = 0.5f;
            DealDamage(targets, GetDamages(), IDamageable.DamageType.magique);
        }
        Destroy(gameObject, 5f);
    }

    public void DealDamage(List<GameObject> targets, float dmg, IDamageable.DamageType typeDmg)
    {        
        foreach(var targ in targets.ToArray())
        {
            if (targ.GetComponent<IDamageable>().team != team)
            {
                targ.GetComponent<IDamageable>().TakeDamage(dmg, typeDmg);
            }
            else
            {
                targ.GetComponent<IDamageable>().SetHealth(targ.GetComponent<IDamageable>().GetHealth() + dmg);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("minion") || other.CompareTag("golem"))
        {
            insideArea += 1;
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.CompareTag("minion") || other.CompareTag("golem"))
        {
            insideArea -= 1;
            targets.Remove(other.gameObject);
        }
    }
}
