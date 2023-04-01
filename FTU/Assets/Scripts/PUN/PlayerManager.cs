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
        player = PV.Controller;
        //index = PlayerPrefs.GetInt("persoID");
    }

    
    private void Start()
    {
        if (PV.IsMine)
        {
        index =(int) PhotonNetwork.LocalPlayer.CustomProperties["_pp"];
            Debug.LogFormat("My team is {0} I am {1} and a play : {2}", player.GetPhotonTeam(), player.NickName, playerPrefabs[index].name);
            CreateController();
        }
    }

    void CreateController()
    {
        GameObject _playerPrefab = PhotonNetwork.Instantiate(playerPrefabs[index].name, new Vector3(0f, 2.14f, 0f), Quaternion.identity, 0);

        _playerPrefab.GetComponent<IDamageable>().teams.Code =(byte) player.CustomProperties["_pt"];

        if (_playerPrefab.GetComponent<IDamageable>().teams.Code == 0)
        {
            _playerPrefab.GetComponent<PlayerStats>().deathPos = new Vector3(313.3f, 2.14f, -37.118f);
            _playerPrefab.GetComponent<PlayerStats>().respawnPos = new Vector3(323.3f, 2.14f, -37.118f);
        }
        else
        {
            _playerPrefab.GetComponent<PlayerStats>().deathPos = new Vector3(-313.3f, 2.14f, -37.118f);
            _playerPrefab.GetComponent<PlayerStats>().respawnPos = new Vector3(-323.3f, 2.14f, -37.118f);
        }

    }
}
