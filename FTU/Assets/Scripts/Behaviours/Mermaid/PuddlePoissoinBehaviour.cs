using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddlePoissoinBehaviour : Projectile
{
    private float cpt;
    public MermaidBehaviour mb;
    // Start is called before the first frame update
    public new void Start()
    {
        cpt = 5.0f;
        StartCoroutine(DestroyBullet(cpt));
    }
    // Update is called once per frame
    public new void Update()
    {
      
    }

    public void DealDamage(List<GameObject> targets, float dmg, IDamageable.DamageType typeDmg)
    {        
        foreach(var targ in targets.ToArray())
        {
            if (targ.GetComponent<IDamageable>().team.Code != team.Code)
            {
                targ.GetComponent<IDamageable>().TakeDamage(dmg, typeDmg,mb.photonView.ViewID);
            }
            else
            {
                targ.GetComponent<IDamageable>().SetHealth(targ.GetComponent<IDamageable>().GetHealth() + dmg);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>())
        {
            StartCoroutine(puddleBehaviour(other.gameObject));
        }
    }


    private IEnumerator puddleBehaviour(GameObject target)
    {
        var cpt = 0;
        while (cpt < 6)
        {
            try
            {
                if(target.GetComponent<IDamageable>().team.Code != team.Code)
                {
                    target.GetComponent<IDamageable>().TakeDamage(GetDamages(), GetDamageType(),mb.photonView.ViewID);
                }
                else
                {
                    target.GetComponent<IDamageable>().SetHealth(target.GetComponent<IDamageable>().GetHealth() + GetDamages());
                }
            }
            catch(MissingReferenceException missE)
            {
                cpt = 10;
            }
            catch(NullReferenceException nullE)
            {
                cpt = 10;
            }
            cpt++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
