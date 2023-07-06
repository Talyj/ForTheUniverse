using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItemScript : MonoBehaviour
{
    public TMP_Text playerName;

    public Image characterSprit;

    public TMP_Text kda;

    private PhotonView player;
    
    public Sprite[] characterImage;
    // Start is called before the first frame update
    void Start()
    {
        playerName.text = player.Controller.NickName;
        characterSprit.sprite =characterImage[player.gameObject.GetComponent<IDamageable>().characterID];
        
    }

    // Update is called once per frame
    void Update()
    {
        kda.text = player.gameObject.GetComponent<PlayerStats>().kill + "/" +
                   player.gameObject.GetComponent<PlayerStats>().death + "/" +
                   player.gameObject.GetComponent<PlayerStats>().assist;
    }
}
