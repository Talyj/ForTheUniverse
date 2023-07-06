using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviourPun
{
    [SerializeField] PhotonTeamsManager manag;
    public PhotonTeam teams;
    public GameObject shopUI;
    public List<PlayerStats> playerPrefab;
    public bool shopIsOpen = false;
    private bool isIn;

    public void Start()
    {
        isIn = false;
        playerPrefab = new List<PlayerStats>();
    }

    private void Update()
    {
        if (isIn)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                shopUI.GetComponent<ShopUI>().teamCode = teams.Code;
                shopIsOpen = !shopIsOpen;
                OpenOrCloseShop();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            if (other.gameObject.GetComponent<IDamageable>().team.Code == teams.Code)
            {
                playerPrefab.Add(other.gameObject.GetComponent<PlayerStats>());
                isIn = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        foreach (var player in playerPrefab)
        {
            if (player.photonView.IsMine)
            {
                if (playerPrefab.Contains(player))
                {
                    playerPrefab.Remove(player);
                    isIn = false;
                    shopIsOpen = !shopIsOpen;
                    shopUI.SetActive(false);
                    return;
                }
            }
        }
    }
    void OpenOrCloseShop()
    {
        if (shopIsOpen == true)
        {

            shopUI.SetActive(true);
        }
        else
        {
            shopUI.SetActive(false);
        }
    }
}
