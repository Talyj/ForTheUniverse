using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    //public Team teams;
    [SerializeField] PhotonTeamsManager manag;
    public PhotonTeam teams;
    [SerializeField] private GameObject mau;
    private bool isAwake;

    public void Start()
    {
        isAwake = false;
        manag = GameObject.Find("RoomManager").GetComponentInChildren<PhotonTeamsManager>();
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
        switch (gameObject.name)
        {
            case "Temple1":
                teams.Code = 1;
                break;

            case "Temple2":
                teams.Code = 0;
                break;

        }
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 50f);

        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
               
                try
                {
                    var playerTeam = col.gameObject.GetComponent<PhotonView>().Owner.GetPhotonTeam().Code;
                        //Debug.Log("<color='green'> "+playerTeam+"</color>");
                    if (col.gameObject.CompareTag("Player") )
                    {
                        if(playerTeam != teams.Code)
                        {
                        //Debug.Log("<color='blue'>God du temple "+gameObject.name +"</color>");
                        SpawnDemiGod();

                        }
                    }
                    else
                    {

                        //Debug.Log("<color='black'>no God" + gameObject.name + "</color>");
                    }
                }
                catch (System.Exception)
                {

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
            semiGod.GetComponent<MauBehaviour>().team = this.teams;
            semiGod.GetComponent<MauBehaviour>().templeTransform = gameObject.transform;
            //Debug.Log("<color='pink'> " + semiGod.GetComponent<MauBehaviour>().teams+ "</color>");
        }
    }
}
