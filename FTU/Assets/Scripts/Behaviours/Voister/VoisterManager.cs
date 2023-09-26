using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoisterManager : MonoBehaviour
{
    //0 == crab, 1 == fox, 2 == slime
    [SerializeField] private GameObject[] voisters;
    [SerializeField] private KingsBehaviour[] kings;
    [HideInInspector] public int food;
    private float cpt;
    private KingsBehaviour currentKing;
    private bool isRespawning;

    void Start()
    {
        isRespawning = false;
        food = 10;
        var randKing = Random.Range(0, 3);
        if (PhotonNetwork.IsMasterClient)
        {
            currentKing = PhotonNetwork.Instantiate(kings[randKing].name, new Vector3(0, 2.5f, 0), Quaternion.identity).GetComponent<KingsBehaviour>();
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (CurrentKingIsDead() && !isRespawning)
            {
                isRespawning = true;
                StartCoroutine(SpawnKings());
            }
        }
    }

    private bool CurrentKingIsDead()
    {
        if (!currentKing | currentKing.GetHealth() <= 0) return true;
        return false;
    }

    public IEnumerator SpawnKings()
    {
        yield return new WaitForSeconds(60);
        var randKing = Random.Range(0, 2);
        currentKing = PhotonNetwork.Instantiate(kings[randKing].name, new Vector3(0, 2.5f, 0), Quaternion.identity).GetComponent<KingsBehaviour>();
        currentKing.voisterManager = this;
        isRespawning = false;

    }
}
