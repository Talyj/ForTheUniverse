using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharmAreaBehaviour : Projectile
{
    public MermaidBehaviour source;

    private new void Start()
    {

        StartCoroutine(DestroyBullet(1f));
    }

    public new void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>() && photonView.IsMine || other.CompareTag("minion") && other.gameObject.GetComponent<IDamageable>() && photonView.IsMine)
        {
            if(other.gameObject.GetComponent<IDamageable>().team != source.team){
                source.AddTCharmedTargets(other.gameObject);
            }
        }
    }
}
