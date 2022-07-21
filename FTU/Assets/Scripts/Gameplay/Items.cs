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

    private void Awake()
    {
        
    }
    private void Start()
    {
        img.sprite = item.img;
        nameItem.text = item.nameItem;
        price.text = item.price.ToString();
    }
    public void BuyItem()
    {
        if(shop.playerPrefab.gold >= item.price)
        {
            shop.playerPrefab.gold -= item.price;
            Debug.Log("Equip " + item.nameItem);
            shop.playerPrefab.items.Add(item);
            shop.playerPrefab.ItemEquip();
        }
        else
        {
            Debug.Log("need gold");
        }
        

    }
}
