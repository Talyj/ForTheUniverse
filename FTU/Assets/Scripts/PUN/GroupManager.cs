using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class GroupManager : MonoBehaviourPunCallbacks
{
    private Dictionary<string, Group> groups = new Dictionary<string, Group>();

    public void JoinGroup(string groupName)
    {
        if (!groups.ContainsKey(groupName))
        {
            // Crée un nouveau groupe avec le joueur en tant que leader
            Group group = new Group(groupName, PhotonNetwork.LocalPlayer);
            groups.Add(groupName, group);
        }
        else
        {
            // Ajoute le joueur à un groupe existant
            Group group = groups[groupName];
            group.AddMember(PhotonNetwork.LocalPlayer);
        }

        // Actualise l'affichage des joueurs dans le groupe
        UpdateGroupUI();
    }

    public void LeaveGroup(string groupName)
    {
        if (groups.ContainsKey(groupName))
        {
            // Supprime le joueur du groupe
            Group group = groups[groupName];
            group.RemoveMember(PhotonNetwork.LocalPlayer);

            // Si le joueur est le leader, détruit le groupe
            if (PhotonNetwork.LocalPlayer.Equals(group.Leader))
            {
                groups.Remove(groupName);
            }

            // Actualise l'affichage des joueurs dans le groupe
            UpdateGroupUI();
        }
    }

    private void UpdateGroupUI()
    {
        // Met à jour l'affichage des joueurs dans le groupe
        // par exemple en utilisant une UI avec une liste de joueurs
    }
}