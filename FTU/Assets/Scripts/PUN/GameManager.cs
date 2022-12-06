using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The list of prefab that represent the differente characters")]
        public GameObject[] playerPrefabs;
        
        private int numberPlayer = 2;

        public PlayerManager selector;
        public void Start()
        {
            if (SceneManager.GetActiveScene().name == "WaitingRoom")
            {
                LoadArena(); 
                return;
            }
            if (playerPrefabs[0] == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if(PlayerMovement.localPlayerInstance == null)
                {
                    //int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
                    selector = GameObject.FindObjectOfType<PlayerManager>();
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    //if(team == 0)
                    //{
                    //    PhotonNetwork.Instantiate(playerPrefabs[selector.selectIndex].name, new Vector3(-313.3f, 2.14f, -37.118f), Quaternion.identity, 0);
                    //}
                    //else
                    //{
                    //    PhotonNetwork.Instantiate(playerPrefabs[selector.selectIndex].name, new Vector3(313.3f, 2.14f, -37.118f), Quaternion.identity, 0);
                    //}
                    PhotonNetwork.Instantiate(playerPrefabs[selector.GetIndex()].name, new Vector3(0f, 2.14f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public void JoinTeam(int team)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
                PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
            }
            else
            {
                ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
                {
                    {"Team", team }
                };
                PhotonNetwork.SetPlayerCustomProperties(playerProps);
            }
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //if (PhotonNetwork.CurrentRoom.PlayerCount >= numberPlayer)
            //{
            //    PhotonNetwork.LoadLevel("MainGameRoom");
            //}
            //else
            //{
            //    PhotonNetwork.LoadLevel("WaitingRoom");
            //}
            PhotonNetwork.LoadLevel("TestIA");
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            Debug.Log($"OnPlayerEnteredRoom() {other.NickName}"); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }
    }
}