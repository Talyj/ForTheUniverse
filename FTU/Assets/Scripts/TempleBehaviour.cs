using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    public IDamageable.Team team;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().team != team)
        {
            Debug.Log("Quelqu'un est entré dans la zone du temple");
        }
    }
}
