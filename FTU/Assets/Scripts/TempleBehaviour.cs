using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    //public Team teams;
    [SerializeField] PhotonTeamsManager manag;
    PhotonTeam team;
    [SerializeField] private GameObject mau;
    private bool isAwake;

    public void Start()
    {
        isAwake = false;
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetInsideTemple();
        }
    }

    private void GetInsideTemple()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 50f);

        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<IDamageable>().teams != team)                    
                {
                    SpawnDemiGod();
                }

            }
        }
    }

    private void SpawnDemiGod()
    {
        if (!isAwake)
        {
            isAwake = true;
            var semiGod = PhotonNetwork.Instantiate(mau.name, new Vector3(gameObject.transform.position.x, 9, gameObject.transform.position.z), Quaternion.identity);
            semiGod.GetComponent<MauBehaviour>().teams = team;
            semiGod.GetComponent<MauBehaviour>().templeTransform = gameObject.transform;
        }
    }
}
