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
    PhotonView pv;
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text textPerso;
    [SerializeField] TMP_Text textTeam;
    [SerializeField] TMP_Dropdown perso;
    [SerializeField] GameObject persoGO;
    [SerializeField] GameObject textPersoGO;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public void SetUp(Player _player)
    {
        
        
        player = _player;
        text.text = _player.NickName;
        PlayerPrefs.SetInt("persoID", perso.value);
        //var test = Enum.GetName(typeof(Team), PlayerPrefs.GetInt("Teams")); 
        textTeam.text = Enum.GetName(typeof(Team), PlayerPrefs.GetInt("Teams"));
        
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

    public string GetTeam()
    {
        
        return textTeam.text;
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
