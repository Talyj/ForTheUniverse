using System.Collections;
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
    }
    
    public GameObject contentFriendList;
    public GameObject buttonFriendList;
    public GameObject controlPanel;
    public GameObject connectionPanel;
    public GameObject characterHolder;
    public TMP_Text profilName;
    
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
        PhotonNetwork.JoinLobby();
        connectionPanel.SetActive(false);
        controlPanel.SetActive(true);
        characterHolder.SetActive(true);
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var btn = Instantiate(buttonFriendList, contentFriendList.transform);
            btn.GetComponent<Button>().onClick.AddListener(() => DisplayFriend(player));
            btn.GetComponentInChildren<TMP_Text>().text = player.UserId;
        }
        //GroupManager.Instance().JoinGroup("Group2");
        //profilName.text = PhotonNetwork.LocalPlayer.NickName;
    }

    private void DisplayFriend(Player player)
    {
        Debug.Log(player.UserId);
    }

    public IEnumerator GetFriends()
    {
        var highscoreURL = "http://awacoru.cluster027.hosting.ovh.net/getFriends.php?user=" +
                           MainMenuManager.Instance().GetLocalPlayer().CustomProperties["idUser"];
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
    }

    public Player GetLocalPlayer()
    {
        return PhotonNetwork.LocalPlayer;
    }

    public void LaunchGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
