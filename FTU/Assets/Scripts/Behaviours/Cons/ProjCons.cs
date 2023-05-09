using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ProjCons : Projectile
{
    public ConsBehaviour source;
    [SerializeField] PhotonTeamsManager manag;
    public PhotonTeam teams;

    public new void Start()
    {
        manag = GameObject.Find("RoomManager").GetComponentInChildren<PhotonTeamsManager>();

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
                if (other.gameObject.GetComponent<IDamageable>().teams != teams)
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
