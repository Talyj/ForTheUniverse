using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public ItemBehaviours item;
    public Shop shop;
    public Image img;
    public Text nameItem,price;
    public PlayerStats stats;

    private void Awake()
    {
        stats = shop.playerPrefab;
    }
    private void Start()
    {
        img.sprite = item.img;
        nameItem.text = item.nameItem;
        price.text = item.price.ToString();
    }
    public void BuyItem()
    {
        if(stats.gold >= item.price)
        {
            stats.gold -= item.price;
            Debug.Log("Equip " + item.nameItem);
            stats.items.Add(item);
            stats.ItemEquip();
        }
        else
        {
            Debug.Log("need gold");
        }
        

    }
}
