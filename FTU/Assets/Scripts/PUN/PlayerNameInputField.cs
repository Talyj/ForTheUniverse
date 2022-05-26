using UnityEngine;
using UnityEngine.UI;


using Photon.Pun;
using Photon.Realtime;


using System.Collections;


namespace Com.MyCompany.MyGame
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";

        public void Start()
        {


            string defaultName = string.Empty;
            Text _text = this.GetComponent<Text>();
            if (_text != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _text.text = defaultName;
                }
            }


            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;


            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}