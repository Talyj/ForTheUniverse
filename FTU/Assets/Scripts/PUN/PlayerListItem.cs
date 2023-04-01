using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using Photon.Pun.UtilityScripts;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public Player player;
    public const string PersoPlayerProp = "_pp";
    [SerializeField] TMP_Text text;
    [SerializeField] Image background;
    [SerializeField] Sprite[] sp;
    //[SerializeField] TMP_Text textPerso;
    //public TMP_Text textTeam;
    //[SerializeField] TMP_Dropdown perso;
    //[SerializeField] GameObject persoGO;
    //[SerializeField] GameObject textPersoGO;
    [SerializeField] PhotonTeamsManager manag;
    PhotonTeam team;

    public void Awake()
    {
        manag = GameObject.Find("RoomManager").GetComponentInChildren<PhotonTeamsManager>();
        background.color = new Color(0f, 0f, 0f, 0f);
    }

    public void SetUp(Player _player)
    {

        player = _player;
        text.text = _player.NickName;
        if (player.GetPhotonTeam() == null)
        {
            Debug.Log($"<color=red> Setup NO TEAM</color>");
        }
        else
        {
            team = _player.GetPhotonTeam();
            Debug.Log($"<color=green> Setup {player.GetPhotonTeam().Name}</color>");

        }
    }
    
    public bool CheckIsMine(Player _player)
    {
        return player.Equals(_player) ;
    }
    public void PersoSelected(Character charac)
    {
        if (player.IsLocal)
        {
            player.SetCustomProperties(new Hashtable { { PersoPlayerProp, charac.characterIndex } });
            //background.sprite = charac.characterSprite;
            //if (player.CustomProperties.ContainsKey("_pp"))
            //{
            //    background.sprite = sp[(int)player.CustomProperties["_pp"]];
            //}
            background.color = new Color(255f, 255f, 255f, 0.25f);
        }
        
    }

    
    public void JoinTeam(Player[] players)
    {
        for (int i = 0; i < players.Count(); i++)
        {
            if (players[i].IsLocal)
            {
                players[i].JoinTeam((byte)((i % 2 == 0) ? 0 : 1));
                if (players[i].GetPhotonTeam() == null)
                {
                    Debug.Log($"<color=red> NO TEAM</color>");
                }
            }
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    if (player == targetPlayer)
    //    {
    //        UpdatePlayerItem(targetPlayer);
    //    }
    //}

    //public void UpdatePlayerItem(Player player)
    //{
    //    if (player.CustomProperties.ContainsKey("_pp"))
    //    {
    //        background.sprite = sp[(int)player.CustomProperties["_pp"]];
    //    }
    //}
    //public void ApplyLocalChange()
    //{
    //    if (player.CustomProperties.ContainsKey("_pp"))
    //    {
    //        background.sprite = sp[(int)player.CustomProperties["_pp"]];
    //    }
    //}

}
