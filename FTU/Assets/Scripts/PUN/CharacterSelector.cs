using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviourPunCallbacks
{

    public GameObject[] listOfCharacter;
    public Transform spwan;
    public Text characterName;
    public int currentIndex;
    public int selectIndex;
    public CharacterSelector instance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        instance = this;
        GameObject other = GameObject.Find("manage");
        if (other.GetComponent<CharacterSelector>().instance != this)
        {
            Destroy(other);
        }
        selectIndex = currentIndex;
        PhotonNetwork.Instantiate(listOfCharacter[currentIndex].gameObject.name, spwan.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name =="CharacterSelection")
        {
            characterName.text = listOfCharacter[currentIndex].name;
        }
        
        

    }

    public void NextButton()
    {
        if(currentIndex >= listOfCharacter.Length-1)
        {
            currentIndex = 0;
            PhotonNetwork.Instantiate(listOfCharacter[currentIndex].gameObject.name, spwan.position, Quaternion.identity);
            GameObject pref = GameObject.Find(listOfCharacter[listOfCharacter.Length - 1].name + "(Clone)").gameObject;
            //Debug.Log(pref);
            Destroy(pref);
        }
        else
        {
            currentIndex += 1;
            PhotonNetwork.Instantiate(listOfCharacter[currentIndex].gameObject.name, spwan.position, Quaternion.identity);
            GameObject pref = GameObject.Find(listOfCharacter[currentIndex - 1].name + "(Clone)").gameObject;
            //Debug.Log(pref);
            Destroy(pref);
        }
        
    }
    public void PreviousButton()
    {
        if(currentIndex <= 0)
        {
            currentIndex = listOfCharacter.Length-1;
            PhotonNetwork.Instantiate(listOfCharacter[listOfCharacter.Length - 1].gameObject.name, spwan.position, Quaternion.identity);
            GameObject pref = GameObject.Find(listOfCharacter[0].name + "(Clone)").gameObject;
            //Debug.Log(pref);
            Destroy(pref);
        }
        else
        {
            currentIndex -= 1;
            PhotonNetwork.Instantiate(listOfCharacter[currentIndex].gameObject.name, spwan.position, Quaternion.identity);
            GameObject pref = GameObject.Find(listOfCharacter[currentIndex + 1].name + "(Clone)").gameObject;
            //Debug.Log(pref);
            Destroy(pref);
        }
        
    }

    public void Select()
    {
        selectIndex = currentIndex;
        GameObject pref = GameObject.Find(listOfCharacter[currentIndex].name + "(Clone)").gameObject;
        //Debug.Log(pref);
        Destroy(pref);
        PhotonNetwork.LoadLevel("MainGameRoom");
    }
}
