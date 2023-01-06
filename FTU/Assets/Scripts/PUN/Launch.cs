using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launch : MonoBehaviourPunCallbacks
{

    public static Launch Instance;
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
    
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            return;
        }
        //if (string.IsNullOrEmpty(playerName.text))
        //{
        //    PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        //}
        //else
        //{
        //    PhotonNetwork.NickName = playerName.text;
        //}
        PhotonNetwork.CreateRoom(roomName.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {

            GameObject pPref = Instantiate(playerListPrefab, playerListContent);
            pPref.GetComponent<PlayerListItem>().SetUp(players[i]);
            if (players.Count() >= 2)
            {
                Debug.Log("creation teams");
                CreateTeams(players);
                pPref.GetComponent<PlayerListItem>().GetTeam();
            }
            else
            {
                Debug.Log("wait a player");
            }
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        //CreateTeams(players);
        //photonView.RPC(nameof(CreateTeams), RpcTarget.All, players);
    }
    

    
    void CreateTeams(Player[] players)
    {
        for (int i = 0; i < players.Count(); i++)
        {
            if (i % 2 == 0)
            {
                players[i].CustomProperties["teams"] = Team.Dominion;
                PlayerPrefs.SetInt("Teams", 0);
                
            }
            else
            {
                players[i].CustomProperties["teams"] = Team.Veritas;
                PlayerPrefs.SetInt("Teams", 1);
            }
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;
        
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
        GameObject npPef = Instantiate(playerListPrefab, playerListContent);
        npPef.GetComponent<PlayerListItem>().SetUp(newPlayer);
        npPef.GetComponent<PlayerListItem>().GetTeam();
        Player[] players = PhotonNetwork.PlayerList;
        
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
