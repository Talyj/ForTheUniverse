using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterSelectorMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject championsMenu;
    
    [SerializeField]
    private GameObject[] champions;

    private int championsSelected = 0;

    public Vector3 championPad;

    private GameObject championPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("championsSelected"))
        {
            championsSelected = PlayerPrefs.GetInt("championsSelected");
        }

        DisplaySelectedChampion();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        DisplaySelectedChampion();
    }

    public void DisplaySelectedChampion()
    {
        if (championPrefab != null)
        {
            Destroy(championPrefab);
        }
        championPrefab = Instantiate(champions[championsSelected], championPad, Quaternion.Euler(0, 0, 0));
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
