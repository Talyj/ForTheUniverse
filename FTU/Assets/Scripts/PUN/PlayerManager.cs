using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject deathPos;

    private GameObject _playerPrefab;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PV.Controller;
        //index = PlayerPrefs.GetInt("championsSelected");
    }

    
    private void Start()
    {
        if (PV.IsMine)
        {
            index =(int) PhotonNetwork.LocalPlayer.CustomProperties["championsSelected"];
            CreateControllerV2();
            //PV.RPC("SyncTeam", RpcTarget.AllBuffered, player.GetPhotonTeam().Name, index);
            Debug.LogFormat("My team is {0} I am {1} and a play : {2}", player.GetPhotonTeam().Name, player.NickName, playerPrefabs[index].name);
        }
        
    }
    
    void CreateControllerV2()
    {
        
        var spawnPos = player.GetPhotonTeam().Code == 0 ? new Vector3(323.3f, 2.14f, -37.118f) : new Vector3(-323.3f, 2.14f, -37.118f);
        //if(player.GetPhotonTeam().Code == 0)
        //{
        //    //_playerPrefab = PhotonNetwork.Instantiate(playerPrefabs[index].name, new Vector3(323.3f, 2.14f, -37.118f), Quaternion.identity, 0);
        //    //_playerPrefab.GetComponent<IDamageable>().team.Code = (byte)player.CustomProperties["_pt"];
        //    //_playerPrefab.GetComponent<IDamageable>().team.Name = player.GetPhotonTeam().Name;
        //    //_playerPrefab.GetComponent<IDamageable>().userId = PhotonNetwork.LocalPlayer.UserId;
        //    //_playerPrefab.GetComponent<PlayerStats>().playerManage = this;
        //}
        //else if(player.GetPhotonTeam().Code == 1)
        //{
        //}
        _playerPrefab = PhotonNetwork.Instantiate(playerPrefabs[index].name, spawnPos, Quaternion.identity, 0);
        _playerPrefab.GetComponent<PlayerStats>().respawnPos = spawnPos;
        _playerPrefab.GetComponent<IDamageable>().team.Code = (byte)player.CustomProperties["_pt"];
        _playerPrefab.GetComponent<IDamageable>().team.Name = player.GetPhotonTeam().Name;
        _playerPrefab.GetComponent<IDamageable>().userId = PhotonNetwork.LocalPlayer.UserId;
        _playerPrefab.GetComponent<PlayerStats>().playerManage = this;
        PV.RPC("SyncTeam", RpcTarget.AllBuffered, player.GetPhotonTeam().Code, index);
    }

    [PunRPC]
    void SyncTeam(byte team, int index)
    {
        var chara = FindObjectsOfType<PlayerStats>();
        Debug.Log(index +"fjffhfhfhfhfhfhf");
        Debug.Log(chara.Length + " count");
        var prefab = chara.First(x => x.characterID == index);
        prefab.team.Code = team;
        prefab.team.Name = player.GetPhotonTeam().Name;
    }


}
