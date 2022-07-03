using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviourPun
{
    //State of the game
    public bool isPlaying;
    private bool isGameStarted;

    //[0] = veritas, [1] = dominion
    public GameObject[] victoryDisplay;

    [SerializeField] private Transform spawnTransformVeritas;
    [SerializeField] private Transform spawnTransformDominion;
    [SerializeField] private Transform deathPos;

    // Start is called before the first frame update
    public void Start()
    {
        isPlaying = false;
        isGameStarted = false;
    }

    // Update is called once per frame
    public void Update()
    {
        //isPlaying = true;
        //isGameStarted = false;
        if (!isPlaying && PhotonNetwork.PlayerList.Length >= 2 && SceneManager.GetActiveScene().name == "MainGameRoom")
        {
            var photonViews = FindObjectsOfType<PhotonView>();
            var players = new List<GameObject>();
            foreach(var view in photonViews)
            {
                var player = view.Owner;

                if (player != null)
                {
                    var playerPrefabObject = view.gameObject;
                    players.Add(playerPrefabObject);
                }
            }
            CreateTeams(players);
        }
        else if (isPlaying && !isGameStarted)
        {
            Game();
        }
        CheckVictory();
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
                //if (monst.team == IDamageable.Team.Veritas)
                //{
                //    victoryDisplay[1].SetActive(true);
                //}
                //else
                //{
                //    victoryDisplay[0].SetActive(true);
                //}
                //Time.timeScale = 0;
            }
        }                
    }

    private void CreateTeams(List<GameObject> players)
    {
        for (var i = 0; i < players.Count; i++)
        {
            if (i % 2 == 0)
            {
                players[i].GetComponent<IDamageable>().team = IDamageable.Team.Dominion;
                players[i].GetComponent<IDamageable>().respawnPos = new Vector3(spawnTransformDominion.position.x, 2.11f, spawnTransformDominion.position.z);
            }
            else
            {
                players[i].GetComponent<IDamageable>().team = IDamageable.Team.Veritas;
                players[i].GetComponent<IDamageable>().respawnPos = new Vector3(spawnTransformVeritas.position.x, 2.11f, spawnTransformVeritas.position.z);
            }
            players[i].GetComponent<IDamageable>().deathPos = deathPos.position;
        }
        isPlaying = true;
        Spawn(players);
    }

    private void Spawn(List<GameObject> players)
    {
        foreach(var play in players)
        {
            if(play.GetComponent<IDamageable>().team == IDamageable.Team.Veritas)
            {
                play.transform.position = new Vector3(spawnTransformVeritas.position.x, 2.11f, spawnTransformVeritas.position.z);
            }
            else
            {
                play.transform.position = new Vector3(spawnTransformDominion.position.x, 2.11f, spawnTransformDominion.position.z);
            }
        }
    }
}
