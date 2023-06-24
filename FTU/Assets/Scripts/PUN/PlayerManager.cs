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

    private GameObject _playerPrefab;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PV.Controller;
        //index = PlayerPrefs.GetInt("persoID");
    }

    
    private void Start()
    {
        if (PV.IsMine)
        {
            index =(int) PhotonNetwork.LocalPlayer.CustomProperties["championsSelected"];
            
            CreateController();
            PV.RPC("SyncTeam", RpcTarget.All, player.GetPhotonTeam() );
        }
        Debug.LogFormat("My team is {0} I am {1} and a play : {2}", player.GetPhotonTeam(), player.NickName, playerPrefabs[index].name);
        
    }

    void CreateController()
    {
        _playerPrefab = PhotonNetwork.Instantiate(playerPrefabs[index].name, new Vector3(0f, 2.14f, 0f), Quaternion.identity, 0);
        
        _playerPrefab.GetComponent<IDamageable>().team.Code =(byte) player.CustomProperties["_pt"];
        _playerPrefab.GetComponent<IDamageable>().userId = player.NickName;
        Debug.Log(_playerPrefab.GetComponent<IDamageable>().userId);

        _playerPrefab.GetComponent<PlayerStats>().playerManage = this;
        if (_playerPrefab.GetComponent<IDamageable>().team.Code == 0)
        {
            _playerPrefab.GetComponent<PlayerStats>().deathPos = new Vector3(413.3f, 2.14f, -37.118f);
            _playerPrefab.GetComponent<PlayerStats>().respawnPos = new Vector3(323.3f, 2.14f, -37.118f);
        }
        else if(_playerPrefab.GetComponent<IDamageable>().team.Code == 1)
        {
            _playerPrefab.GetComponent<PlayerStats>().deathPos = new Vector3(-413.3f, 2.14f, -37.118f);
            _playerPrefab.GetComponent<PlayerStats>().respawnPos = new Vector3(-323.3f, 2.14f, -37.118f);
        }

    }
    
    [PunRPC]
    void SyncTeam(byte team)
    {
        _playerPrefab.GetComponent<IDamageable>().team.Code =(byte) team;
    }

    
}
