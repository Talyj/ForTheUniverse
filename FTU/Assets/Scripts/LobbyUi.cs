using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyUi : MonoBehaviourPunCallbacks
{
    public List<Character> characters = new List<Character>();
    public Transform holderCellPrefabs;
    public GameObject cellPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Character character in characters)
        {
            SpwanCharacterCell(character);
        }
    }

    private void SpwanCharacterCell(Character character)
    {
        GameObject chracterCell = Instantiate(cellPrefabs, holderCellPrefabs);

        Image splashArt = chracterCell.GetComponent<Image>();
        TMP_Text nameCharacter = chracterCell.transform.Find("Name").GetComponent<TMP_Text>();

        splashArt.sprite = character.characterSprite;
        nameCharacter.text = character.characterName;


        //centrer l'image
        Vector2 pixelSize = new Vector2(splashArt.sprite.texture.width, splashArt.sprite.texture.height);
        Vector2 pixelPivot = splashArt.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelPivot.y, pixelSize.x / pixelSize.y);

        splashArt.GetComponent<RectTransform>().pivot = uiPivot;

        chracterCell.GetComponent<Button>().onClick.AddListener(()=>GetCharacter(character));
    }

    public void GetCharacter(Character character)
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                var plI = GameObject.FindObjectOfType<PlayerListItem>();
                Debug.Log($" perso : {character.characterName} index: {character.characterIndex} <color=blue>  {player.Value.NickName}</color> <color=green>  {player.Value}</color>");

                //PhotonView pv = plI.GetComponent<PhotonView>();

                //pv.RPC("RPC_PersoSelected", RpcTarget.All, character);
                plI.PersoSelected(character);
                //plI.ApplyLocalChange();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
