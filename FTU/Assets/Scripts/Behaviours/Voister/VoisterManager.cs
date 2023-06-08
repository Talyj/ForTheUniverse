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

    void Start()
    {
        cpt = 5;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if (/*SceneManager.GetActiveScene().name == "MainGameRoom"*/ true)
            {
                if (/*mainGame.isPlaying && */cpt <= 0)
                {
                    cpt = 60;
                    for(int i = 0; i < kings.Length; i++)
                    {
                        SpawnVoisters(voisters[i], kings[i]);
                    }
                }
            }
        }
    }

    private void SpawnVoisters(GameObject voister, KingsBehaviour king)
    {
        var x = Random.Range(-150, 150);
        var z = Random.Range(-50, 50);
        var occurence = Random.Range(1, 3);
        for(int i = 0; i < occurence; i++)
        {
            var voisterTemp = PhotonNetwork.Instantiate(voister.name, new Vector3(x, king.transform.position.y, z), Quaternion.identity);
            voisterTemp.GetComponent<VoisterBehaviour>().kingVoisters = king;
        }
    }
}
