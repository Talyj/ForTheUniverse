using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    Player player;
    [SerializeField] int index;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        
    }

    public int GetIndex()
    {
        return index;
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            //CreateController();
            Debug.Log(index);
        }
    }

    void CreateController()
    {
        index = (int)player.CustomProperties["persoID"];
    }
}
