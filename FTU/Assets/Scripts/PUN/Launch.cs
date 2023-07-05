using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    [SerializeField] Transform playerListContentInRoom;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] PlayerListItem _playerListPrefab;
    [SerializeField] List<PlayerListItem> playerList =new List<PlayerListItem>();
    [SerializeField] GameObject startGameButton;

    public float timeBeetweenUpdate = 1.5f;
    float nextUpdateTime;
    
    
    //Matchmaking Variable
    public const string ELO_PROP_KEY = "C0";
    private TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);
    private LoadBalancingClient loadBalancingClient;

    private void Awake()
    {
        Instance = this;


        PhotonTeamsManager.PlayerJoinedTeam += OnPlayerJoinedTeam;
        PhotonTeamsManager.PlayerLeftTeam += OnPlayerLeftTeam;
    }
    void Start()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        LobbyJoin();
        // creer nouvelle fonction avec joinorcreate
        //PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        

    }

    //public void SetName()
    //{
    //    PhotonNetwork.NickName = playerName.text;
    //    Debug.Log(PhotonNetwork.NickName +" name set");
    //}
    
    public void LobbyJoin()
    {
        /*RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.CustomRoomProperties = new Hashtable { { ELO_PROP_KEY, PhotonNetwork.LocalPlayer.CustomProperties["elo_score"] } };
        roomOptions.CustomRoomPropertiesForLobby = new string[]{ ELO_PROP_KEY}; // makes "C0" and "C1" available in the lobby
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = "AWA";
        enterRoomParams.RoomOptions = roomOptions;
        enterRoomParams.Lobby = sqlLobby;
        //enterRoomParams.Lobby = PhotonNetwork.CurrentLobby;
        //loadBalancingClient = new LoadBalancingClient();
        //loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
        //PhotonNetwork.JoinOrCreateRoom("AWA", new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true },PhotonNetwork.CurrentLobby);
        PhotonNetwork.JoinRandomOrCreateRoom(roomName:"AWA", roomOptions:roomOptions,typedLobby:sqlLobby);*/
        StartCoroutine(GetScore());
        //JoinRandomRoom();

    }
    public void CreateRoom()
    {
        /*if (string.IsNullOrEmpty(roomName.text))
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + " room",new RoomOptions() { MaxPlayers=4,BroadcastPropsChangeToAll=true});
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName.text);
        }*/
        //Debug.Log(PhotonNetwork.NickName);
        
        Debug.Log("create room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.CustomRoomProperties = new Hashtable { { ELO_PROP_KEY, PhotonNetwork.LocalPlayer.CustomProperties["elo_score"] } };
        roomOptions.CustomRoomPropertiesForLobby = new string[]{ ELO_PROP_KEY}; // makes "C0" and "C1" available in the lobby
        /*EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = "AWA";
        enterRoomParams.RoomOptions = roomOptions;
        enterRoomParams.Lobby = sqlLobby;*/
        PhotonNetwork.CreateRoom(roomName:"AWA " + PhotonNetwork.LocalPlayer.NickName, roomOptions:roomOptions,typedLobby:sqlLobby);
        
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
        Debug.Log("on room");
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties[ELO_PROP_KEY]);
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        

            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        UpdatePlayerList();
    }

    private float AverageELORoom()
    {
        float res = 0;
        foreach (var p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            res += (float)p.CustomProperties["elo_score"];
        }

        return res / PhotonNetwork.CurrentRoom.Players.Count;
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.CurrentRoom.CustomProperties[ELO_PROP_KEY] = AverageELORoom();
        Debug.LogError(PhotonNetwork.CurrentRoom.CustomProperties[ELO_PROP_KEY]);
        Debug.Log("join room+ " + newPlayer.NickName);
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.CustomProperties[ELO_PROP_KEY] = AverageELORoom();
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties[ELO_PROP_KEY]);
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
        //MenuManager.Instance.OpenMenu("loading");
        PhotonNetwork.LoadLevel(0);
        //UpdatePlayerList();
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        //MenuManager.Instance.OpenMenu("loading");
        UpdatePlayerList();

    }

    public bool JoinRandomRoom()
    {
        float elo = (float)PhotonNetwork.LocalPlayer.CustomProperties["elo_score"];
        float[] boundaries = new[] { elo - 15, elo + 15 };
        string sql = $"C0 BETWEEN {boundaries[0].ToString("F2", CultureInfo.InvariantCulture)} AND {boundaries[1].ToString("F2", CultureInfo.InvariantCulture)}";
        Debug.LogError(sql);
        return PhotonNetwork.JoinRandomRoom(new Hashtable() {}, 4, MatchmakingMode.FillRoom, sqlLobby, sql);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
        CreateRoom();
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
        //Debug.Log($"<color=green> before clear :  {playerList.Count}</color>");
        foreach (PlayerListItem item in playerList)
        {
            Destroy(item.gameObject);
        }
        playerList.Clear();
        //Debug.Log($"<color=yellow> after clear :  {playerList.Count}</color>");

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerListItem newPlayerItem = Instantiate(_playerListPrefab, playerListContentInRoom);
            

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.SetUp(player.Value);
                newPlayerItem.JoinTeam(players);
            }
            playerList.Add(newPlayerItem);
        }
        //Debug.Log($"<color=blue> after potential ADD clear :  {playerList.Count}</color>");
        Invoke("SetTeams", 0.1f);
        //photonView.RPC("SetTeams", RpcTarget.All);
    }

    [PunRPC]
    public void SetTeams()
    {
        foreach (Transform player in playerListContentInRoom)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();
        Player[] players = PhotonNetwork.PlayerList;
        for(int i =0;i< players.Count(); i++)
        {
            
            if (players[i].GetPhotonTeam().Code == 0)
            {
                PlayerListItem newPlayerItem= Instantiate(_playerListPrefab, playerListContentInRoom);
                newPlayerItem.SetUp(players[i]);
                playerList.Add(newPlayerItem);
            }
            else if(players[i].GetPhotonTeam().Code == 1)
            {

                PlayerListItem newPlayerItem=Instantiate(_playerListPrefab, playerListContentInRoom);
                newPlayerItem.SetUp(players[i]);
                playerList.Add(newPlayerItem);
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
        PhotonNetwork.LoadLevel(2);

    }


    public IEnumerator GetScore()
    {
        int idUser = (int)PhotonNetwork.LocalPlayer.CustomProperties["idUser"];
        int idCharacter = (int)PhotonNetwork.LocalPlayer.CustomProperties["championsSelected"] + 1;
        
        var score = $"http://awacoru.cluster027.hosting.ovh.net/computeScore.php?user={idUser}&chara={idCharacter}";
        WWW request = new WWW(score);
        yield return request;
        
        if (request.error != null)
        {
            print("There was an error getting the high score: " + request.error);
        }
        else
        {
            if (request.text != "500")
            {
                PlayerScore ps = JsonUtility.FromJson<PlayerScore>(request.text);
                PhotonNetwork.LocalPlayer.CustomProperties["elo_score"] = ps.score;
                Debug.LogError(ps.score);
                JoinRandomRoom();
            }
        }
        
    }
}


public struct PlayerScore
{
    public float score;
}