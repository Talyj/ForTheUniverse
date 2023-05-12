using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class CharacterSelectorMenu : MonoBehaviourPunCallbacks
{
    public GameObject menu;
    public GameObject championsMenu;
    
    [SerializeField]
    private GameObject[] champions;

    private int championsSelected = 0;

    public GameObject[] championPad;
    private int slot = -1;
    private int versionGroup = 0;
    private bool groupReady = false;

    private GameObject championPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.HasKey("championsSelected"))
        {
            championsSelected = PlayerPrefs.GetInt("championsSelected");
        }
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add("championsSelected",championsSelected);
        MainMenuManager.Instance().GetLocalPlayer().SetCustomProperties(customProp);

        
        DisplaySelectedChampion();
    }

    // Update is called once per frame
    void Update()
    {
        if (GroupManager.Instance().GetGroup() != null)
        {
            slot = GroupManager.Instance().GetGroup().GetSlotNumber(PhotonNetwork.LocalPlayer);
            Destroy(championPrefab);
        }
        
    }

    public void ToggleChampionsMenu()
    {
        menu.SetActive(!menu.activeSelf);
        championsMenu.SetActive(!championsMenu.activeSelf);
    }

    public void ChangeSelectedCharacter(int index)
    {
        championsSelected = index;
        PlayerPrefs.SetInt("championsSelected", index);
        ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
        customProp.Add("championsSelected",index);
        MainMenuManager.Instance().GetLocalPlayer().SetCustomProperties(customProp);

        if (GroupManager.Instance().GetGroup() == null)
        {
            DisplaySelectedChampion();
        }
    }

    public void DisplaySelectedChampion()
    {
        var slotToDisplay = slot;
        if (slot < 0)
        {
            slotToDisplay = 0;
        }

        if (championPrefab != null)
        {
            Destroy(championPrefab);
        }
        championPrefab = Instantiate(champions[championsSelected], championPad[slotToDisplay].transform.position + new UnityEngine.Vector3(0,1.16f, 0), Quaternion.Euler(0, 0, 0));
        foreach (var monoBehaviourPun in championPrefab.GetComponents<MonoBehaviourPun>())
        {
            monoBehaviourPun.enabled = false;
        }
        championPrefab.GetComponent<UI>().enabled = false;
        championPrefab.GetComponent<CameraWork>().enabled = false;
        championPrefab.GetComponent<Targeting>().enabled = false;
        championPrefab.GetComponent<Animator>().enabled = false;
    }
    
    
}
