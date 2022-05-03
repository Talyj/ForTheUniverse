using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsBehaviour : IDamageable
{
    public GameObject xp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead() == true)
        {
            ExpFor();
        }
    }

    public void ExpFor()
    {
        Instantiate(xp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
