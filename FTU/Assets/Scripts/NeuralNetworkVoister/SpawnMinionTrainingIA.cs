using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinionTrainingIA : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject minion;
    [SerializeField] private Transform[] spawnPoint;
    public Transform[] path0;
    public Transform[] path1;
    public Transform[] path2;
    public Transform[] path3;
    public Transform[] path4;
    public Transform[] path5;

    public List<Transform[]> paths = new List<Transform[]>();

    private float cpt;

    // Start is called before the first frame update
    void Start()
    {
        cpt = 2;
        paths.Add(path0);
        paths.Add(path1);
        paths.Add(path2);
        paths.Add(path3);
        paths.Add(path4);
        paths.Add(path5);
    }

    // Update is called once per frame
    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if (cpt <= 0)
            {
                cpt = 30;
                for(int j = 0; j < spawnPoint.Length; j++)
                {
                    var randPos = Random.Range(0, 3);                    
                    for (int i = 0; i <= 10; i++)
                    {
                        //SetMinions(BasicAIStats.Way.up, Team.Veritas, i, paths[j][randPos], paths[j], randPos);
                    }
                }
            }
        }
    }

    public void SetMinions(BasicAIStats.Way way, PhotonTeam team, int loopCounter, Transform spawnPos, Transform[] path, int currentPos)
    {
        //Vector3 spawn = spawnPos.position;
        var x = Random.Range(-3, 3);
        var Z = Random.Range(-3, 3);

        var color = new Color();

        //if (team == Team.Veritas)
        //{
        //    spawn = spawnPoint[0].position + new Vector3(x, 0, Z);
        //    color = Color.yellow;
        //}
        //else
        //{
        //    spawn = spawnPoint[1].position + new Vector3(x, 0, Z);
        //    color = Color.red;
        //}

        var minionTemp = PhotonNetwork.Instantiate(minion.name, spawnPos.position, Quaternion.identity);
        minionTemp.GetComponent<MinionsBehaviour>().way = way;
        minionTemp.GetComponent<MinionsBehaviour>().team = team;
        minionTemp.GetComponent<MinionsBehaviour>().targetsUp = path;
        minionTemp.gameObject.GetComponent<Renderer>().material.color = color;
        minionTemp.GetComponent<MinionsBehaviour>().current = currentPos;
        if (loopCounter > 6)
        {
            minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Ranged;
        }
        else minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Melee;
    }
}
