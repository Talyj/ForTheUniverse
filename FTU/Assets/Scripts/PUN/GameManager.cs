using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The list of prefab that represent the differente characters")]
        public GameObject[] playerPrefabs;
        
        private int numberPlayer = 2;

        public CharacterSelector selector;
        public void Start()
        {
            if (SceneManager.GetActiveScene().name == "WaitingRoom") return;
            if (playerPrefabs[0] == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if(PlayerMovement.localPlayerInstance == null)
                {
                    selector = GameObject.FindObjectOfType<CharacterSelector>();
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(playerPrefabs[selector.selectIndex].name, new Vector3(0f, 2.14f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
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
            if (PhotonNetwork.CurrentRoom.PlayerCount <= numberPlayer)
            {
                PhotonNetwork.LoadLevel("WaitingRoom");
            }
            else
            {
                PhotonNetwork.LoadLevel("MainGameRoom");
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


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