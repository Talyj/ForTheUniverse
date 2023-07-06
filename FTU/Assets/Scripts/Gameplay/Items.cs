using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviourPun
{
    public ItemStats item;
    public Shop shop;
    public Image img;
    public Text nameItem, price;
    public List<PlayerStats> stats;

    private void Start()
    {
        stats = shop.playerPrefab;
        img.sprite = item.img;
        nameItem.text = item.nameItem;
        price.text = item.price.ToString();
    }
    public void BuyItem()
    {
        foreach (var player in stats)
        {
            if (player.photonView.IsMine)
            {
                if (player.gold >= item.price && CanBuy(player, item))
                {
                    player.gold -= item.price;
                    Debug.Log("Equip " + item.nameItem);

                    player.items.Add(item, 1);
                    player.CallItemEquip();
                }
            }
        }
    }

    private bool CanBuy(PlayerStats player, ItemStats item)
    {
        //Check conso
        if(item.rarete == ItemRarete.Consommable)
        {
            if (player.items.ContainsKey(item))
            {
                player.items[item] += 1;
                return false;
            }
        }
        //Check qty
        if (player.items.Count > 4) return false;
        //Check Duplicate
        if (player.items.ContainsKey(item))
        {
            return false;
        }
        return true; ;
    }
}
