using Photon.Pun;
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
            if(SceneManager.GetActiveScene().name == "MainGameRoom")
            {
                if (mainGame.isPlaying && cpt <= 2)
                {
                    cpt = 30;
                    for (int i = 0; i <= 4; i++)
                    {
                        SetMinions(BasicAIStats.Way.up, Team.Veritas, i);
                        SetMinions(BasicAIStats.Way.down, Team.Veritas, i);

                        SetMinions(BasicAIStats.Way.up, Team.Dominion, i);
                        SetMinions(BasicAIStats.Way.down, Team.Dominion, i);
                    }
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
        minionTemp.GetComponent<MinionsBehaviour>().targetsDown = pathDown;
        minionTemp.gameObject.GetComponent<Renderer>().material.color = color;
        if(loopCounter > 2)
        {
            minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Ranged;
        }
        else minionTemp.GetComponent<MinionsBehaviour>().attackType = IDamageable.AttackType.Melee;
    }
}
