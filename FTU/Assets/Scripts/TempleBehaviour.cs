using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBehaviour : MonoBehaviour
{
    public IDamageable.Team team;
    [SerializeField] private GameObject mau;
    private bool isAwake;

    public void Start()
    {
        isAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<IDamageable>().team != team)
        {
            Debug.Log("Quelqu'un est entré dans la zone du temple");
            SpawnDemiGod();
        }
    }

    private void SpawnDemiGod()
    {
        if (!isAwake)
        {
            isAwake = true;
            var semiGod = PhotonNetwork.Instantiate(mau.name, new Vector3(gameObject.transform.position.x, 10, gameObject.transform.position.z), Quaternion.identity);
            semiGod.GetComponent<MauBehaviour>().team = team;
            semiGod.GetComponent<MauBehaviour>().templeTransform = gameObject.transform;
        }
    }
}
