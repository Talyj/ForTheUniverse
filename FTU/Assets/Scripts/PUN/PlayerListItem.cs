using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    public TMP_Text textTeam;
    public TMP_Dropdown perso;
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
        PlayerPrefs.SetInt("persoID", perso.value);
        //var test = Enum.GetName(typeof(Team), PlayerPrefs.GetInt("Teams")); 
        textTeam.text = Enum.GetName(typeof(Team), PlayerPrefs.GetInt("Teams")); 

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public void PersoSelected()
    {

        PlayerPrefs.SetInt("persoID", perso.value);
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
