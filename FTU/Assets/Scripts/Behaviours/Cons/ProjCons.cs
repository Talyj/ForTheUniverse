using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjCons : Projectile
{
    public ConsBehaviour source;
    public Team team;

    public new void Start()
    {
        StartCoroutine(DestroyBullet());
    }


    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>())
        {
            if (other.GetComponent<IDamageable>().team != team)
            {
                source.AddPassive();
                DealDamage(other.gameObject, GetDamages(), GetDamageType());
                stopProjectile = true;
                Destroy(gameObject);
            }
        }

    }

}
