using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharmAreaBehaviour : MonoBehaviourPun
{
    public MermaidBehaviour source;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(DestroyBullet());
        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f);
        if (photonView.IsMine)
        {            
            PhotonNetwork.Destroy(gameObject);
        }
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
