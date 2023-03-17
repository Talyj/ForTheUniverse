using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Photon.Pun.UtilityScripts;

public class Launch : MonoBehaviourPunCallbacks
{

    public static Launch Instance;
    [SerializeField] PhotonTeamsManager manag;
    PhotonTeam team;
    [SerializeField] TMP_InputField roomName;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_Text error;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startGameButton;
    

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("connecting to master..");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("base");
        Debug.Log("Joined lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        Debug.Log(PhotonNetwork.NickName);
    }

    public void SetName()
    {
        //PhotonNetwork.NickName = playerName.text;
        Debug.Log(PhotonNetwork.NickName +" name set");
    }
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + " room");
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName.text);
        }
        //if (string.IsNullOrEmpty(playerName.text))
        //{
        //    PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        //}
        //else
        //{
        //    PhotonNetwork.NickName = playerName.text;
        //}
        //Debug.Log(PhotonNetwork.NickName);
        Player[] players = PhotonNetwork.PlayerList;
        MenuManager.Instance.OpenMenu("loading");

    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        PhotonTeamsManager.PlayerJoinedTeam += OnPlayerJoinedTeam;
        PhotonTeamsManager.PlayerLeftTeam += OnPlayerLeftTeam;
        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < players.Count(); i++)
        {
            players[i].JoinTeam((byte)((i % 2 == 0) ? 0 : 1));

            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        //photonView.RPC(nameof(CreateTeams), RpcTarget.All, players);
    }
    
    private void OnPlayerJoinedTeam(Player player, PhotonTeam team)
    {
        Debug.LogFormat("Player {0} joined team {1}", player, team);
        Debug.Log(player.GetPhotonTeam());
        //GameObject p = Instantiate(playerListPrefab, playerListContent);
        //p.GetComponent<PlayerListItem>().SetUp(player);
        //p.GetComponent<PlayerListItem>().MyTeam(player);
    }
    private void OnPlayerLeftTeam(Player player, PhotonTeam team)
    {
        Debug.LogFormat("Player {0} left team {1}", player, team);
    }

    void CreateTeams(Player[] players)
    {
        for (int i = 0; i < players.Count(); i++)
        {
            if (i % 2 == 0)
            {
                //players[i].CustomProperties["teams"] = Team.Dominion;
                //PlayerPrefs.SetInt("Teams", 0);
                players[i].JoinTeam(0);


            }
            else
            {
                //players[i].CustomProperties["teams"] = Team.Veritas;
                //PlayerPrefs.SetInt("Teams", 1);
                players[i].JoinTeam(1);
            }
        }
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log(otherPlayer.NickName + " left team");
        otherPlayer.LeaveCurrentTeam();
        
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        error.text = "Error: " +message; 
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
             Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            

        }

    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("base");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        Debug.Log("join room+ " + newPlayer.NickName);
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        Player[] players = PhotonNetwork.PlayerList;
        //CreateTeams(players);
        
    }


    public void StartGame()
    {
        //Player[] players = PhotonNetwork.PlayerList;
        //switch (players.Count())
        //{
        //    case 2:
        //        //load scene 1v1
        //        break;
        //    case > 2:
        //        //load scene base
        //        break;
        //    case 1:
        //        //load scene traning
        //        break;
        //}

        //PhotonNetwork.LoadLevel(1);
    }
}
