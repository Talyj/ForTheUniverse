using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviourPun
{
    [SerializeField] PhotonTeamsManager manag;
    public PhotonTeam teams;
    public GameObject shopUI;
    public PlayerStats playerPrefab;
    public bool shopIsOpen = false;
    private bool isIn;

    public void Start()
    {
        isIn = false;
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
                playerPrefab = other.gameObject.GetComponent<PlayerStats>();
                isIn = true;
            }
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerStats>() == playerPrefab)
        {
            isIn = false;
            shopIsOpen = !shopIsOpen;
            shopUI.SetActive(false);
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
