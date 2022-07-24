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

    public new void Update()
    {
        //:)
    }


    private IEnumerator DestroyBullet()
    {
        if (photonView.IsMine)
        {
            yield return new WaitForSeconds(3f);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.GetComponent<IDamageable>())
            {
                if (other.gameObject.GetComponent<IDamageable>().team != team)
                {
                    source.AddPassive();
                    DealDamage(other.gameObject, GetDamages(), GetDamageType());
                    stopProjectile = true;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

    }

}
