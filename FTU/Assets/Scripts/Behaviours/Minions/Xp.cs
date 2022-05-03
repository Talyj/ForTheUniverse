using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>())
        {
            other.gameObject.GetComponent<IDamageable>().SetExp(60);
        }
    }
}
