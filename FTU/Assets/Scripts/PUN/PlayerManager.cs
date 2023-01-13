using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Player player;
    [SerializeField] int index;
    [Tooltip("The list of prefab that represent the differente characters")]
    public GameObject[] playerPrefabs;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        index = PlayerPrefs.GetInt("persoID");
        player = PV.Controller;
    }

    
    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
            Debug.LogFormat("My team is {0} I am {1} and a play : {2}", player.GetPhotonTeam(), player.NickName, playerPrefabs[index].name);
        }
    }

    void CreateController()
    {
        //playerPrefabs[index].GetComponent<IDamageable>().team = (Team)PlayerPrefs.GetInt("Teams");
        var team = player.GetPhotonTeam();
        playerPrefabs[index].GetComponent<IDamageable>().teams = team;
        
        if (team.Code == 0)
        {
            playerPrefabs[index].GetComponent<PlayerStats>().respawnPos = new Vector3(313.3f, 2.14f, -37.118f);
        }
        else
        {
            playerPrefabs[index].GetComponent<PlayerStats>().respawnPos = new Vector3(-313.3f, 2.14f, -37.118f);
        }
        PhotonNetwork.Instantiate(playerPrefabs[index].name, new Vector3(0f, 2.14f, 0f), Quaternion.identity, 0);
        //if(playerPrefabs[index].GetComponent<IDamageable>().team == Team.Dominion)
        //{
        //    playerPrefabs[index].GetComponent<PlayerStats>().respawnPos = new Vector3(313.3f, 2.14f, -37.118f);
        //    //playerPrefabs[index].GetComponent<IDamageable>().respawnPos = new Vector3(325.3f, 2.14f, -37.118f);
        //}
        //else
        //{
        //    playerPrefabs[index].GetComponent<PlayerStats>().respawnPos = new Vector3(-313.3f, 2.14f, -37.118f);
        //    //playerPrefabs[index].GetComponent<IDamageable>().respawnPos = new Vector3(310.3f, 2.14f, -37.118f);
        //}

    }
}
