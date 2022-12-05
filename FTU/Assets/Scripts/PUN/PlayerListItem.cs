using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    public TMP_Dropdown perso;
    ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
        //customProps["persoID"] = perso.value;
        //Debug.Log(customProps["persoID"]+ "setup");
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
        Debug.Log(customProps["persoID"]  + "customprop");
        Debug.Log(perso.value + "ddlval");
        customProps["persoID"] = perso.value;
        PhotonNetwork.LocalPlayer.CustomProperties = customProps;
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
