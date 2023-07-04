using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball2 : Projectile
{

    private new void Start()
    {
        StartCoroutine(DestroyBullet(3f));
    }

    public ActioBehaviour dps;
    private void OnCollisionEnter(Collision col)
    {
        if (photonView.IsMine)
        {
            float dmg = dps.skills[0].Damage;
            if (col.gameObject.GetComponent<IDamageable>())
            {
                if(col.gameObject.GetComponent<IDamageable>().team != dps.team)
                {
                    if (col.gameObject.GetComponent<IDamageable>().GetControl() == IDamageable.ControlType.slow)
                    {
                        dmg *= 1.15f;
                        col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats,dps.photonView.ViewID);
                        Debug.Log("<color=green> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
                    }
                    else
                    {
                        col.gameObject.GetComponent<IDamageable>().TakeDamage(dmg, dps.skills[0].degats, dps.photonView.ViewID);
                        Debug.Log("<color=blue> damage: </color>" + dmg + " " + dps.skills[0].degats.ToString());
                    }

                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
