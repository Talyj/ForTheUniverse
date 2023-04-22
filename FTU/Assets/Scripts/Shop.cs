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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().teams== teams)
        {
            playerPrefab = other.gameObject.GetComponent<PlayerStats>();
            if (Input.GetKeyDown(KeyCode.P))
            {
                shopIsOpen = !shopIsOpen;
                OpenOrCloseShop();
            }
            Debug.Log("in shop");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().teams == teams)
        {
            playerPrefab = other.gameObject.GetComponent<PlayerStats>();
            if (Input.GetKeyDown(KeyCode.P))
            {
                shopIsOpen = !shopIsOpen;
                OpenOrCloseShop();
                
            }
            
        }
    }
    private void Update()
    {
        
    }

    
    private void OnTriggerExit(Collider other)
    {
        shopIsOpen = false;
        OpenOrCloseShop();
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
