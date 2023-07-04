using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ConnectionToServer : MonoBehaviour
{
    public TMP_InputField usernameInput;    
    public TMP_InputField passwordInput;


    public void ConnectionHander()
    {
        StartCoroutine(TryConnection());
    }
    public IEnumerator TryConnection()
    {
        
        /*if (usernameInput.text.Length > 0)
        {
            PhotonNetwork.LocalPlayer.NickName = usernameInput.text;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        }*/
        
        Debug.Log(usernameInput.text);
        Debug.Log(passwordInput.text);
        var highscoreURL = "http://localhost/ftu/PHP/accessServer.php?user=" +
                              usernameInput.text + "&password=" +
                              passwordInput.text;
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            if (hs_get.text == "200")
            {
                MainMenuManager.Instance().Connect();
                PhotonNetwork.LocalPlayer.NickName = usernameInput.text;
            }
        }
        yield return null;
    }
}
