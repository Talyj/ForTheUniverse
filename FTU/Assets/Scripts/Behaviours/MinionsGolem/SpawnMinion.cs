using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnMinion : MonoBehaviour
{
    [SerializeField] private GameObject minion;
    [SerializeField] private Transform[] spawnPoint;
    public Transform[] pathUp;
    public Transform[] pathDown;

    private MainGame mainGame;
    private float cpt;
    //SpawnPoint[0] = Gauche = Veritas
    //SpawnPoint[1] = Droite = Dominion
    // Start is called before the first frame update
    void Start()
    {
        mainGame = GetComponent<MainGame>();
        cpt = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if(/*SceneManager.GetActiveScene().name == "MainGameRoom"*/ true)
            {
                if (/*mainGame.isPlaying && */cpt <= 2)
                {
                    cpt = 120;
                    for (int i = 0; i < 3; i++)
                    {
                        //1 == veritas , 0 == Dominion
                        SetMinions(BasicAIStats.Way.up, 1, i);
                        SetMinions(BasicAIStats.Way.down, 1, i);

                        SetMinions(BasicAIStats.Way.up, 0, i);
                        SetMinions(BasicAIStats.Way.down, 0, i);
                    }
                }
            }
        }
    }

    public void SetMinions(BasicAIStats.Way way, int team, int loopCounter)
    {
        Vector3 spawn;
        var x = Random.Range(-10, 10);
        var Z = Random.Range(-10, 10);

        var color = new Color();

        if (team == 1)
        {
            spawn = spawnPoint[0].position + new Vector3(x, 3, Z);
            color = Color.yellow;
        }
        else
        {
            spawn = spawnPoint[1].position + new Vector3(x, 3, Z);
            color = Color.red;
        }

        var minionTemp = PhotonNetwork.Instantiate(minion.name, spawn, Quaternion.identity);
        minionTemp.GetComponent<MinionsBehaviour>().way = way;
        switch (team)
        {
            case 0:
                minionTemp.GetComponent<MinionsBehaviour>().team.Name = "Dominion";
                break;
            case 1:
                minionTemp.GetComponent<MinionsBehaviour>().team.Name = "Veritas";
                break;
            case 2:
                minionTemp.GetComponent<MinionsBehaviour>().team.Name = "Voister";
                break;
        }
        minionTemp.GetComponent<MinionsBehaviour>().team.Code = (byte)team;
        minionTemp.GetComponent<MinionsBehaviour>().targetsUp = pathUp;
        minionTemp.GetComponent<MinionsBehaviour>().targetsDown = pathDown;
        minionTemp.gameObject.GetComponent<Renderer>().material.color = color;
        if (loopCounter > 2)
        {
            minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Ranged;
        }
        else minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Melee;
    }
}
