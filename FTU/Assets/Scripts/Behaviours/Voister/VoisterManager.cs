using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoisterManager : MonoBehaviour
{
    //0 == crab, 1 == fox, 2 == slime
    [SerializeField] private GameObject[] voisters;
    [SerializeField] private KingsBehaviour[] kings;
    private float cpt;
    private KingsBehaviour currentKing;
    private bool isRespawning;

    void Start()
    {
        isRespawning = false;
        var randKing = Random.Range(0, 2);
        if (PhotonNetwork.IsMasterClient)
        {
            currentKing = PhotonNetwork.Instantiate(kings[randKing].name, new Vector3(0, 2.5f, 0), Quaternion.identity).GetComponent<KingsBehaviour>();
        }
    }

    void Update()
    {
        //TODO Move this inside the kings script
        if (PhotonNetwork.IsMasterClient)
        {
            //cpt -= Time.deltaTime;
            //if (/*SceneManager.GetActiveScene().name == "MainGameRoom"*/ true)
            //{
            //    if (/*mainGame.isPlaying && */cpt <= 0)
            //    {
            //        cpt = 60;
            //        for(int i = 0; i < kings.Length; i++)
            //        {
            //            //SpawnVoisters(voisters[i], kings[i]);
            //        }
            //    }
            //}
            if (CheckCurrentKing() && !isRespawning)
            {
                isRespawning = true;
                StartCoroutine(SpawnKings());
            }
        }
    }

    private bool CheckCurrentKing()
    {
        if (!currentKing | currentKing.GetHealth() <= 0) return true;
        return false;
    }

    public IEnumerator SpawnKings()
    {
        yield return new WaitForSeconds(60);
        var randKing = Random.Range(0, 2);
        currentKing = PhotonNetwork.Instantiate(kings[randKing].name, new Vector3(0, 2.5f, 0), Quaternion.identity).GetComponent<KingsBehaviour>();
        isRespawning = false;

    }
}
