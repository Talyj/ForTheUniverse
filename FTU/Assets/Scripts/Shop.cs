using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Team team;
    public ItemBehaviours[] allItems;
    public GameObject shopUI;
    public GameObject itemPrefab;
    public bool shopIsOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().team == team)
        {
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
        if(other.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().team == team)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                shopIsOpen = !shopIsOpen;
                OpenOrCloseShop();
                
            }
            if (Input.GetKeyDown(KeyCode.O)&& shopIsOpen==true)
            {
                Debug.Log("buy");
                other.gameObject.GetComponent<PlayerStats>().items.Add(allItems[0]);
                other.gameObject.GetComponent<PlayerStats>().ItemEquip();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        shopIsOpen = false;
    }
    public void BuyItem()
    {

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
