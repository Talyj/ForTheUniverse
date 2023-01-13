using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text textPerso;
    public TMP_Text textTeam;
    [SerializeField] TMP_Dropdown perso;
    [SerializeField] GameObject persoGO;
    [SerializeField] GameObject textPersoGO;


    
    public void SetUp(Player _player)
    {

        player = _player;
        text.text = _player.NickName;
        PlayerPrefs.SetInt("persoID", perso.value);

        if (player.IsLocal)
        {
            persoGO.SetActive(true);
            textPersoGO.SetActive(false);
        }
        else
        {
            persoGO.SetActive(false);
            textPersoGO.SetActive(true);
            textPerso.text = perso.value.ToString();
        }
    }

    public void PersoSelected()
    {
        PlayerPrefs.SetInt("persoID", perso.value);
    }
    public string MyTeam(Player[] player)
    {
        string txt = "";
        for (int i = 0; i < player.Count(); i++)
        {
            if(player.Count() >= 2)
            {
                txt= textTeam.text = (player[i].GetPhotonTeam().Name == null) ? "" : player[i].GetPhotonTeam().Name;

            }
            else
            {
                txt= "XXX";
            }
        }
        return txt;
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
}
