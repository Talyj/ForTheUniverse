using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidMap : MonoBehaviour
{
    public GameObject[] items;
    public float EventTime,baseTime;
    public int maxItem = 12,currentItem=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EventTime > 0)
        {
            EventTime -= 1 * Time.deltaTime;
        }
        
        if(EventTime <= 0 && currentItem != maxItem)
        {
            VoidEvent();
            EventTime = baseTime;
        }
    }

    void VoidEvent()
    {
        int numberObjectSpawn = 0;
        while (numberObjectSpawn < 6)
        {
            int index = Random.Range(0, items.Length);
            Vector3 randomPos = new Vector3(Random.Range(-230, 230), 2, Random.Range(-90, 90));
            PhotonNetwork.Instantiate(items[index].name, randomPos, Quaternion.identity);
            numberObjectSpawn++;
            currentItem++;
        }
    }
}
