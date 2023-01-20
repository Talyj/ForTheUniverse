using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball1 : Projectile
{
    public Dps1 dps;

    private new void Start()
    {
        StartCoroutine(DestroyBullet(3f));
    }

    private void OnCollisionEnter(Collision col)
    {
        if (photonView.IsMine)
        {
            if (col.gameObject.GetComponent<IDamageable>())
            {
                if (col.gameObject.GetComponent<IDamageable>().team != dps.team)
                {
                    col.gameObject.GetComponent<IDamageable>().TakeCC(IDamageable.ControlType.slow, 2.55f);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
