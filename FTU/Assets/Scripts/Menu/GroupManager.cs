using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;

public class GroupManager : MonoBehaviourPunCallbacks
{
    private static GroupManager _singleton;
    
    public static GroupManager Instance()
    {
        return _singleton;
    }

    private void Awake()
    {
        if (_singleton)
        {
            return;
        }
        _singleton = this;
    }

    private string groupName = null;
    [SerializeField]
    private GameObject[] champions;
    public GameObject[] championPad;
    private GameObject[] championPrefab = new GameObject[5]{null, null, null, null, null};

    private Dictionary<string, Group> groups = new Dictionary<string, Group>();

    public void JoinGroup(string groupName)
    {
        PhotonNetwork.JoinOrCreateRoom(groupName,  new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true },PhotonNetwork.CurrentLobby);
    }

    public void LeaveGroup(string groupName)
    {
        PhotonNetwork.LeaveRoom();
    }

    public void UpdateGroupUI()
    {
        // Met à jour l'affichage des joueurs dans le groupe
        // par exemple en utilisant une UI avec une liste de joueurs
        for (int i = 0; i < 5; i++)
        {
            if (championPrefab[i] != null)
            {
                Destroy(championPrefab[i]);
                championPrefab[i] = null;
            }

            if (PhotonNetwork.CurrentRoom.Players.Count <= i)
            {
                return;
            }
            var player = PhotonNetwork.CurrentRoom.Players.Values.ToArray()[i];
            print(champions[(int)player.CustomProperties["championsSelected"]]);
            
            
            championPrefab[i] = Instantiate(champions[(int)player.CustomProperties["championsSelected"]], championPad[i].transform.position + new Vector3(0,1.16f, 0), Quaternion.Euler(0, 0, 0));
            
            foreach (var monoBehaviourPun in championPrefab[i].GetComponents<MonoBehaviourPun>())
            {
                monoBehaviourPun.enabled = false;
            }
            championPrefab[i].GetComponent<UI>().enabled = false;
            championPrefab[i].GetComponent<CameraWork>().enabled = false;
            championPrefab[i].GetComponent<Targeting>().enabled = false;
            championPrefab[i].GetComponent<Animator>().enabled = false;
        }
    }

    public Group GetGroup()
    {
        if (groupName == null)
        {
            return null;
        }
        return groups[groupName];
    }

    public override void OnCreatedRoom()
    {
        // Crée un nouveau groupe avec le joueur en tant que leader
        groupName = PhotonNetwork.CurrentRoom.Name;

        Debug.LogError("Room created : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        // Ajoute le joueur à un groupe existant
        groupName = PhotonNetwork.CurrentRoom.Name;

        Group group = null;
        if (!groups.ContainsKey(groupName))
        {
            group = new Group(groupName);
            groups.Add(groupName, group);

            group.Leader = PhotonNetwork.CurrentRoom.GetPlayer(0);
            foreach (var players in PhotonNetwork.CurrentRoom.Players.Values)
            {
                group.AddMember(players);
            }
        }
        else
        {
            group = groups[groupName];
            group.AddMember(PhotonNetwork.LocalPlayer);
        }

        Debug.LogError("Room joined : " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogError("Number player room : " + PhotonNetwork.CurrentRoom.Players.Count);
        
        // Actualise l'affichage des joueurs dans le groupe
        UpdateGroupUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Actualise l'affichage des joueurs dans le groupe
        UpdateGroupUI();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Actualise l'affichage des joueurs dans le groupe
        UpdateGroupUI();
    }

    public override void OnLeftRoom()
    {
        
        // Supprime le joueur du groupe
        Group group;

        if (groups.TryGetValue(groupName, out group))
        {
            group.RemoveMember(PhotonNetwork.LocalPlayer);

            // Si le joueur est le leader, détruit le groupe
            if (PhotonNetwork.LocalPlayer.Equals(group.Leader))
            {
                groups.Remove(groupName);
            }
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Actualise l'affichage des joueurs dans le groupe
        UpdateGroupUI();
    }
}