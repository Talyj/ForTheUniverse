using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinionTrainingIA : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject minion;
    [SerializeField] private Transform[] spawnPoint;
    public Transform[] pathUp;

    private float cpt;

    // Start is called before the first frame update
    void Start()
    {
        cpt = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if (cpt <= 0)
            {
                cpt = 30;
                for (int i = 0; i <= 30; i++)
                {
                    SetMinions(BasicAIStats.Way.up, Team.Veritas, i);
                }
            }
        }
    }

    public void SetMinions(BasicAIStats.Way way, Team team, int loopCounter)
    {
        Vector3 spawn = new Vector3(0, 0, 0);
        var x = Random.Range(-10, 10);
        var Z = Random.Range(-10, 10);

        var color = new Color();

        if (team == Team.Veritas)
        {
            spawn = spawnPoint[0].position + new Vector3(x, 0, Z);
            color = Color.yellow;
        }
        else
        {
            spawn = spawnPoint[1].position + new Vector3(x, 0, Z);
            color = Color.red;
        }

        var minionTemp = PhotonNetwork.Instantiate(minion.name, spawn, Quaternion.identity);
        minionTemp.GetComponent<MinionsBehaviour>().way = way;
        minionTemp.GetComponent<MinionsBehaviour>().team = team;
        minionTemp.GetComponent<MinionsBehaviour>().targetsUp = pathUp;
        minionTemp.gameObject.GetComponent<Renderer>().material.color = color;
        if (loopCounter > 9)
        {
            minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Ranged;
        }
        else minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Melee;
    }
}
