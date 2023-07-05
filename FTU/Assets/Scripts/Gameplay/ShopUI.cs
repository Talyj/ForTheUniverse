using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviourPun
{
    public GridLayoutGroup content;
    private ItemPassifs itemPassifs;
    public GameObject itemToSpawn;
    public Shop shop;
    public int teamCode;

    // Start is called before the first frame update
    void Start()
    {
        itemPassifs = FindObjectOfType<ItemPassifs>();
        SetItemsInShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetItemsInShop()
    {
        foreach(var item in itemPassifs.listItems)
        {
            var itemTemp = Instantiate(itemToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
            itemTemp.GetComponent<Items>().item = item;
            itemTemp.GetComponent<Items>().shop = shop;
            itemTemp.transform.SetParent(content.transform, false);
            itemTemp.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
