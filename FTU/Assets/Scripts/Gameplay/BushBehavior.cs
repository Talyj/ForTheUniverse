using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushBehavior : MonoBehaviour
{
    public int bushID;
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable entity;
        if (other.TryGetComponent<IDamageable>(out entity))
        {
            Debug.Log("InBush");
            entity.EnterBush(this);
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable entity;
        if (other.TryGetComponent<IDamageable>(out entity))
        {
            Debug.Log("OutBush");
            entity.ExitBush(this);
            
        }
    }
}
