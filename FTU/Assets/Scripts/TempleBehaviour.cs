using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    //public Team teams;
    [SerializeField] PhotonTeamsManager manag;
    public PhotonTeam team;
    [SerializeField] private GameObject mau;
    private bool isAwake;
    public List<GolemBehaviour> golems;
    public BoxCollider mauCollider;

    public void Start()
    {
        isAwake = false;
        manag = GameObject.Find("RoomManager").GetComponentInChildren<PhotonTeamsManager>();
        golems = new List<GolemBehaviour>(FindObjectsOfType<GolemBehaviour>().Where(x => x.team.Code == team.Code));
    }

    public void Update()
    {
        if(PhotonNetwork.IsMasterClient) CheckGolems();
    }

    private bool CheckGolems()
    {
        if(golems.Count < 1)
        {
            return true;
        }

        foreach(var gol in golems)
        {
            if (gol == null)
            {
                golems.Remove(gol);
                return false;
            }
        }
        return false;
    }




    private void GetInsideTemple()
    {
        //switch (gameObject.name)
        //{
        //    case "Temple1":
        //        teams.Code = 1;
        //        break;

        //    case "Temple2":
        //        teams.Code = 0;
        //        break;

        //}
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
                        if(playerTeam != team.Code)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>())
        {
            if(other.GetComponent<IDamageable>().team.Code != team.Code)
            {
                if (CheckGolems())
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
            mauCollider.enabled = true;
            //var semiGod = PhotonNetwork.Instantiate(mau.name, new Vector3(gameObject.transform.position.x, 9, gameObject.transform.position.z), Quaternion.identity);
            //semiGod.GetComponent<MauBehaviour>().team = this.team;
            //semiGod.GetComponent<MauBehaviour>().templeTransform = gameObject.transform;
            //Debug.Log("<color='pink'> " + semiGod.GetComponent<MauBehaviour>().teams+ "</color>");
        }
    }
}
