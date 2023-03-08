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
    [SerializeField] Transform playerListContentDom;
    [SerializeField] Transform playerListContentVer;
    [SerializeField] Transform playerListContentInRoom;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] PlayerListItem _playerListPrefab;
    [SerializeField] List<PlayerListItem> playerList =new List<PlayerListItem>();
    [SerializeField] GameObject startGameButton;

    public float timeBeetweenUpdate = 1.5f;
    float nextUpdateTime;
    

    private void Awake()
    {
        Instance = this;


        PhotonTeamsManager.PlayerJoinedTeam += OnPlayerJoinedTeam;
        PhotonTeamsManager.PlayerLeftTeam += OnPlayerLeftTeam;
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
        //Debug.Log(PhotonNetwork.NickName);
    }

    //public void SetName()
    //{
    //    PhotonNetwork.NickName = playerName.text;
    //    Debug.Log(PhotonNetwork.NickName +" name set");
    //}
    
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
            //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        Debug.Log(PhotonNetwork.NickName);
        MenuManager.Instance.OpenMenu("loading");

    }
    private void OnPlayerJoinedTeam(Player player, PhotonTeam team)
    {
        Debug.LogFormat("Player {0} joined team {1}", player, team);
        Debug.Log(player.GetPhotonTeam().Name);
    }
    private void OnPlayerLeftTeam(Player player, PhotonTeam team)
    {
        Debug.LogFormat("Player {0} left team {1}", player, team);
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
       
            
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log("join room+ " + newPlayer.NickName);
        UpdatePlayerList();
        //SetTeams();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left team");
        otherPlayer.LeaveCurrentTeam();
        UpdatePlayerList();
        
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


    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("base");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        for (int i = 0; i < roomList.Count; i++)
        {
            
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }


   

    void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (PlayerListItem player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            var newPlayerItem = Instantiate(_playerListPrefab, playerListContentInRoom);
            newPlayerItem.SetUp(player.Value);
            playerList.Add(newPlayerItem);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.JoinTeam(players);
            }
        }
        Invoke("SetTeams", 0.1f);
    }

    public void SetTeams()
    {
        foreach (Transform player in playerListContentInRoom)
        {
            Destroy(player.gameObject);
        }
        Player[] players = PhotonNetwork.PlayerList;
        for(int i =0;i< players.Count(); i++)
        {
            
            if (players[i].GetPhotonTeam().Code == 0)
            {
                Instantiate(playerListPrefab, playerListContentDom).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            else if(players[i].GetPhotonTeam().Code == 1)
            {

                Instantiate(playerListPrefab, playerListContentVer).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
        }
        

    }

    public void StartGame()
    {
        //Player[] players = PhotonNetwork.PlayerList;
        //switch (players.Count())
        //{
        //    case 2:
        //        //load scene 1v1
        //        PhotonNetwork.LoadLevel(2);
        //        break;
        //    case > 2:
        //        //load scene base
        //        PhotonNetwork.LoadLevel(1);
        //        break;
        //    case 1:
        //        //load scene traning
        //        break;
        //}
        PhotonNetwork.LoadLevel(1);

    }
}
