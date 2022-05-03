using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField] private GameObject minion;
    [SerializeField] private Transform[] spawnPoint;
    public Transform[] pathUp;
    public Transform[] pathDown;

    public bool isPlaying;
    //SpawnPoint[0] = Gauche = Veritas
    //SpawnPoint[1] = Droite = Dominion
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        StartCoroutine(SpawnMinionWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnMinionWave()
    {
        while (isPlaying)
        {
            //for (int i = 0; i <= 3; i++)
            //{
                SetMinions(PlayerStats.Way.up, IDamageable.Team.Veritas);
                SetMinions(PlayerStats.Way.down, IDamageable.Team.Veritas);

                SetMinions(PlayerStats.Way.up, IDamageable.Team.Dominion);
                SetMinions(PlayerStats.Way.down, IDamageable.Team.Dominion);
                //yield return new WaitForSeconds(1);
            //}
            yield return new WaitForSeconds(30);
        }
        yield return 0;
    }

    public void SetMinions(PlayerStats.Way way, IDamageable.Team team)
    {
        Vector3 spawn = new Vector3(0, 0, 0);
        if(team == IDamageable.Team.Veritas)
        {
            spawn = spawnPoint[0].position;
        }
        else spawn = spawnPoint[1].position;

        var minionTemp = Instantiate(minion, spawn, Quaternion.identity);
        minionTemp.GetComponent<MinionsBehaviour>().way = way;
        minionTemp.GetComponent<MinionsBehaviour>().team = team;
        minionTemp.GetComponent<MinionsBehaviour>().targetsUp = pathUp;
        minionTemp.GetComponent<MinionsBehaviour>().targetsDown = pathDown;
        minionTemp.GetComponent<MinionsBehaviour>().isAI = true;
    }
}
