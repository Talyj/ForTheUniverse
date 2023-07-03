using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ProjCons : Projectile
{
    public ConsBehaviour source;

    public new void Start()
    {
        StartCoroutine(DestroyBullet(3f));
    }

    public new void Update()
    {
        //:)
    }


    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.GetComponent<IDamageable>())
            {
                if (other.gameObject.GetComponent<IDamageable>().team.Code != team.Code)
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
