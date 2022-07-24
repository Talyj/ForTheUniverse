using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehaviour : Projectile
{
    public TamiBehaviour source;
    public Team team;
    //public Transform targetTransform;
    public new void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    public new void Update()
    {
        //transform.LookAt(targetTransform);
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            var trig = other.gameObject.GetComponent<IDamageable>();
            if (trig)
            {
                if (trig.team != source.team)
                {
                    DealDamage(other.gameObject, GetDamages(), GetDamageType());
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
