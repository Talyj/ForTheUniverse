using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviourPun
{
    public ItemStats item;
    public Shop shop;
    public Image img;
    public Text nameItem,price;
    public PlayerStats stats;
    public int teamCode;

    private void Start()
    {
        shop = FindObjectsOfType<Shop>().Single(x => x.teams.Code == teamCode);
        stats = shop.playerPrefab;
        img.sprite = item.img;
        nameItem.text = item.nameItem;
        price.text = item.price.ToString();
    }
    public void BuyItem()
    {
        if (stats.photonView.IsMine)
        {
            if(stats.gold >= item.price)
            {
                stats.gold -= item.price;
                Debug.Log("Equip " + item.nameItem);
                stats.items.Add(item);
                stats.CallItemEquip();
            }
            else
            {
                Debug.Log("need gold");
            }
        }
        

    }
}
