using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviourPun
{
    //State of the game
    public bool isPlaying;
    private bool isGameStarted;

    //[0] = veritas, [1] = dominion
    public GameObject[] victoryDisplay;

    [SerializeField] public Transform spawnTransformVeritas;
    [SerializeField] public Transform spawnTransformDominion;
    [SerializeField] private Transform deathPos;
    //Dominion = 0,
    //Veritas = 1,
    //Voister = 2
    public Material[] materialsMinimapView;

    public GameObject canvas;
    public TMP_Text textVictory;

    public void Start()
    {
        isPlaying = false;
        isGameStarted = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if(PhotonNetwork.IsMasterClient) isPlaying = true;
        //isGameStarted = false;
        //if (!isPlaying && PhotonNetwork.PlayerList.Length >= 2 && SceneManager.GetActiveScene().name == "MainGameRoom")
        //{
        //    var photonViews = FindObjectsOfType<PhotonView>();
        //    var players = new List<GameObject>();
        //    foreach(var view in photonViews)
        //    {
        //        var player = view.Owner;

        //        if (player != null)
        //        {
        //            try
        //            {
        //                if(view.gameObject.GetComponent<IDamageable>().GetEnemyType() == IDamageable.EnemyType.player)
        //                {
        //                    var playerPrefabObject = view.gameObject;
        //                    players.Add(playerPrefabObject);
        //                }
        //            }
        //            catch(NullReferenceException nullE)
        //            {
        //                // :)
        //            }
        //        }
        //    }
        //    //if(players.Count >= 2)
        //    //{
        //        //CreateTeams(players);
        //    //}
        //}
        //else if (isPlaying && !isGameStarted)
        //{
        //    //Game();
        //}
        //CheckVictory();
    }

    public void SendVictoryMessage(int teamCode)
    {
        if (isPlaying)
        {
            photonView.RPC("CheckVictory", RpcTarget.All, teamCode);
        }
    }

    [PunRPC]
    public void CheckVictory(int teamCode)
    {
        isPlaying = false;
        Time.timeScale = 0.1f;

        Player player = PhotonNetwork.LocalPlayer;
        if (player.GetPhotonTeam().Code == teamCode)
        {
            //Defeat
            textVictory.text = "Defeat";
        }
        else
        {
            //Victory
            textVictory.text = "Victory";
        }
        canvas.SetActive(true);
    }

    [PunRPC]
    public void ResetTime()
    {
        Time.timeScale = 1f;
    }

    public void ReturnButton()
    {
        photonView.RPC("CheckVictory", RpcTarget.All, new object[] { });
        PhotonNetwork.LeaveRoom();
    }
}
