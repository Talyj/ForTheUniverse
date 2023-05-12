using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    private static MainMenuManager _singleton;
    
    public static MainMenuManager Instance()
    {
        return _singleton;
    }

    private void Awake()
    {
        if (_singleton)
        {
            return;
        }
        _singleton = this;
        Connect();
    }
    
    public GameObject contentFriendList;
    public GameObject buttonFriendList;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1";
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        
        GroupManager.Instance().JoinGroup("Group1");
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var btn = Instantiate(buttonFriendList, contentFriendList.transform);
            btn.GetComponent<Button>().onClick.AddListener(() => DisplayFriend(player));
            btn.GetComponentInChildren<TMP_Text>().text = player.UserId;
        }
    }

    private void DisplayFriend(Player player)
    {
        Debug.Log(player.UserId);
    }

    public Player GetLocalPlayer()
    {
        return PhotonNetwork.LocalPlayer;
    }
}
