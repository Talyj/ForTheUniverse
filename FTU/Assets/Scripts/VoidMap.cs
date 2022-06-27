using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidMap : MonoBehaviour
{
    public GameObject[] items;
    public float EventTime,baseTime;
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
        
        if(EventTime <= 0)
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
            Vector3 randomPos = new Vector3(Random.Range(-60, 60), 2, Random.Range(-30, 30));
            Instantiate(items[index], randomPos, Quaternion.identity);
            numberObjectSpawn++;
        }
    }
}
