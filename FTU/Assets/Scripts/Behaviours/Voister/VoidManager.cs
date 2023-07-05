using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VoidManager : MonoBehaviour
{
    public GameObject red_asteroid; // 0
    public GameObject blue_asteroid; // 1

    public VisualEffect meteore;

    private float cpt;
    public int nbAsteroide;

    // Start is called before the first frame update
    void Start()
    {
        cpt = 5;
    }

    // Update is called once per frame
    void Update()
    {
        cpt -= Time.deltaTime;
        if (cpt <= 0 && nbAsteroide < 10)
        {
            cpt = 180;
            var qtyRand = Random.Range(1, 6);
            for (int i = 0; i < qtyRand; i++)
            {
                var x = Random.Range(-175f, 175f);
                var z = Random.Range(-80f, 80f);
                var type = Random.Range(0, 2);

                VisualEffect meteoretutu = Instantiate(meteore, new Vector3(x, -2.24f, z), Quaternion.identity);
                StartCoroutine(DestroyAnimation(meteoretutu, 5.0f));
                if (type == 0)
                {
                    StartCoroutine(SpawnAsteroidAfterDelay(red_asteroid, new Vector3(x, 1.702078f, z), type, 3.2f));
                }
                else if (type == 1)
                {
                    StartCoroutine(SpawnAsteroidAfterDelay(blue_asteroid, new Vector3(x, 1.702078f, z), type, 3.2f));
                }
            }
        }
    }

    private IEnumerator DestroyAnimation(VisualEffect vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(vfx.gameObject);
    }

    private IEnumerator SpawnAsteroidAfterDelay(GameObject asteroid, Vector3 position, int type, float delay)
    {
        yield return new WaitForSeconds(delay);
        var tempAst = PhotonNetwork.Instantiate(asteroid.name, position, Quaternion.identity);
        tempAst.GetComponent<AsteroidsBehaviour>().asteroidType = type;
        tempAst.GetComponent<IDamageable>().team.Code = 2;
        nbAsteroide++;
    }
}
