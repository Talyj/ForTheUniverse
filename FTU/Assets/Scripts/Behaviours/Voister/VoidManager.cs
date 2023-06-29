using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidManager : MonoBehaviour
{
    public GameObject asteroid;
    private float cpt;

    // Start is called before the first frame update
    void Start()
    {
        cpt = 60;
    }

    // Update is called once per frame
    void Update()
    {
        cpt -= Time.deltaTime;
        if (cpt <= 0)
        {
            cpt = 180;
            var qtyRand = Random.Range(1, 6);
            for(int i = 0; i < qtyRand; i++)
            {
                var x = Random.Range(-175f, 175f);
                var y = Random.Range(60f, 150f);
                var z = Random.Range(-80f, 80f);
                var type = Random.Range(0, 2);
                SpawnAsteroid(asteroid, new Vector3(x, y, z), type);
            }


        }
    }

    public void SpawnAsteroid(GameObject asteroid, Vector3 position, int type)
    {
        var tempAst = PhotonNetwork.Instantiate(asteroid.name, position, Quaternion.identity);
        tempAst.GetComponent<AsteroidsBehaviour>().asteroidType = type;
        tempAst.GetComponent<IDamageable>().team.Code = 2;
    }
}
