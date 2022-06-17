using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviourPun
{
    //State of the game
    public bool isPlaying;
    private bool isGameStarted;

    //[0] = veritas, [1] = dominion
    public GameObject[] victoryDisplay;

    // Start is called before the first frame update
    public void Start()
    {
        isPlaying = false;
        isGameStarted = false;
    }

    // Update is called once per frame
    public void Update()
    {
        isPlaying = true;
        isGameStarted = false;
        if (!isPlaying && PhotonNetwork.PlayerList.Length >= 2)
        {
            CreateTeams();
        }
        else if (isPlaying && !isGameStarted)
        {
            Game();
        }
    }

    private void Game()
    {
        isGameStarted = true;
        //TODO Spawn player

        //TODO Spawn minion

        //Check game state (how many golems are destroyed, is the semi god awake ? someone in the temple ?...)    
        CheckVictory();
    }

    private void CheckVictory()
    {
        var Boss = FindObjectsOfType<MauBehaviour>();

        foreach(var monst in Boss)
        {
            if (monst.GetHealth() <= 0)
            {
                if (monst.team == IDamageable.Team.Veritas)
                {
                    victoryDisplay[1].SetActive(true);
                }
                else
                {
                    victoryDisplay[0].SetActive(true);
                }
                Time.timeScale = 0;
            }
        }                
    }

    private void CreateTeams()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (var i = 0; i < players.Length; i++)
        {
            if (i >= players.Length / 2)
            {
                players[i].GetComponent<IDamageable>().team = IDamageable.Team.Dominion;
            }
            else
            {
                players[i].GetComponent<IDamageable>().team = IDamageable.Team.Veritas;
            }
        }
        isPlaying = true;
    }
}
