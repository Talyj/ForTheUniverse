using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        cpt -= Time.deltaTime;
        if (mainGame.isPlaying && cpt <= 0)
        {
            cpt = 30;
            for (int i = 0; i <= 3; i++)
            {
                SetMinions(PlayerStats.Way.up, IDamageable.Team.Veritas);
                SetMinions(PlayerStats.Way.down, IDamageable.Team.Veritas);

                SetMinions(PlayerStats.Way.up, IDamageable.Team.Dominion);
                SetMinions(PlayerStats.Way.down, IDamageable.Team.Dominion);
            }
        }
    }

    public void SetMinions(PlayerStats.Way way, IDamageable.Team team)
    {
        Vector3 spawn = new Vector3(0, 0, 0);
        if(team == IDamageable.Team.Veritas)
        {
            spawn = spawnPoint[0].position;
        }
        else spawn = spawnPoint[1].position;

        var minionTemp = PhotonNetwork.Instantiate(minion.name, spawn, Quaternion.identity);
        minionTemp.GetComponent<MinionsBehaviour>().way = way;
        minionTemp.GetComponent<MinionsBehaviour>().team = team;
        minionTemp.GetComponent<MinionsBehaviour>().targetsUp = pathUp;
        minionTemp.GetComponent<MinionsBehaviour>().targetsDown = pathDown;
        minionTemp.GetComponent<MinionsBehaviour>().isAI = true;
    }
}
