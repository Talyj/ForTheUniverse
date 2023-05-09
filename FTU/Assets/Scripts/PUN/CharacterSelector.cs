using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Com.MyCompany.MyGame;

public class CharacterSelector : MonoBehaviourPunCallbacks
{

    //public GameObject[] listOfCharacter;
    public Transform spwan;
    public Text characterName;
    public int currentIndex;
    public int selectIndex;
    public CharacterSelector instance;
    public Launcher launch;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        instance = this;
        //if (SceneManager.GetActiveScene().name == "Launcher")
        //{
        GameObject other = GameObject.Find("Manage");
        //if (other.GetComponent<CharacterSelector>().instance != this)
        //{
        //    Destroy(other);
        //}
        //}
        selectIndex = currentIndex;
       //Instantiate(listOfCharacter[currentIndex].gameObject, spwan.position, Quaternion.Euler(0f, 180f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        //if(SceneManager.GetActiveScene().name =="Launcher")
        //{
        //    characterName.text = listOfCharacter[currentIndex].name;
        //}
        
        

    }

    //public void NextButton()
    //{
    //    if(currentIndex >= listOfCharacter.Length-1)
    //    {
    //        currentIndex = 0;
    //        Instantiate(listOfCharacter[currentIndex].gameObject, spwan.position, Quaternion.Euler(0f, 180f, 0f));
    //        GameObject pref = GameObject.Find(listOfCharacter[listOfCharacter.Length - 1].name + "(Clone)").gameObject;
    //        //Debug.Log(pref);
    //        Destroy(pref);
    //    }
    //    else
    //    {
    //        currentIndex += 1;
    //        Instantiate(listOfCharacter[currentIndex].gameObject, spwan.position, Quaternion.Euler(0f, 180f, 0f));
    //        GameObject pref = GameObject.Find(listOfCharacter[currentIndex - 1].name + "(Clone)").gameObject;
    //        //Debug.Log(pref);
    //        Destroy(pref);
    //    }
    //    selectIndex = currentIndex;
    //}
    //public void PreviousButton()
    //{
    //    if(currentIndex <= 0)
    //    {
    //        currentIndex = listOfCharacter.Length-1;
    //       Instantiate(listOfCharacter[listOfCharacter.Length - 1].gameObject, spwan.position, Quaternion.Euler(0f, 180f, 0f));
    //        GameObject pref = GameObject.Find(listOfCharacter[0].name + "(Clone)").gameObject;
    //        //Debug.Log(pref);
    //        Destroy(pref);
    //    }
    //    else
    //    {
    //        currentIndex -= 1;
    //        Instantiate(listOfCharacter[currentIndex].gameObject, spwan.position, Quaternion.Euler(0f,180f,0f));
    //        GameObject pref = GameObject.Find(listOfCharacter[currentIndex + 1].name + "(Clone)").gameObject;
    //        //Debug.Log(pref);
    //        Destroy(pref);
    //    }
    //    selectIndex = currentIndex;
    //}

    public void Select()
    {
        //selectIndex = currentIndex;
        //GameObject pref = GameObject.Find(listOfCharacter[currentIndex].name + "(Clone)").gameObject;
        //Destroy(pref);
        launch.Connect();
    }

}
