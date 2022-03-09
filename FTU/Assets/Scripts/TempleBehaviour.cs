using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            Debug.Log("Quelqu'un est entré dans la zone du temple");
        }
    }
}
