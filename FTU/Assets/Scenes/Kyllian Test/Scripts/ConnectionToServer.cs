using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;


public struct PlayerInfoDatabase
{
    public int id;
    public string name;
    public int score;
}
public class ConnectionToServer : MonoBehaviour
{
    public TMP_InputField usernameInput;    
    public TMP_InputField passwordInput;
    public TMP_Text errorMessage;
    public TMP_Text profilName;

    public GameObject formLogin;
    public GameObject loadingImage;

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
        
        /*Debug.Log(usernameInput.text);
        Debug.Log(passwordInput.text);*/
        var highscoreURL = "http://awacoru.cluster027.hosting.ovh.net/accessServer.php?user=" +
                              usernameInput.text + "&password=" +
                              passwordInput.text;
        WWW hs_get = new WWW(highscoreURL);
        formLogin.SetActive(false);
        loadingImage.SetActive(true);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            if (hs_get.text != "500")
            {
                PlayerInfoDatabase playerInfoDatabase = JsonUtility.FromJson<PlayerInfoDatabase>(hs_get.text);
                MainMenuManager.Instance().Connect();
                PhotonNetwork.LocalPlayer.NickName = playerInfoDatabase.name;
                //AuthenticationValues.UserId = int.Parse(hs_get.text);
                profilName.text = playerInfoDatabase.name;
                ExitGames.Client.Photon.Hashtable customProp = new ExitGames.Client.Photon.Hashtable();
                customProp.Add("idUser",playerInfoDatabase.id);
                customProp.Add("elo_score",playerInfoDatabase.score);
                MainMenuManager.Instance().GetLocalPlayer().SetCustomProperties(customProp);
                
            }
            else
            {
                formLogin.SetActive(true);
                loadingImage.SetActive(false);
                errorMessage.gameObject.SetActive(true);
                errorMessage.text = "Erreur : Nom d'utilisateur ou mot de passe erroné.";
            }
        }
        yield return null;
    }
}
