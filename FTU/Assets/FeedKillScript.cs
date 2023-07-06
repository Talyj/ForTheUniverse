using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class FeedKillScript : MonoBehaviour
{
    [SerializeField] public Image leftSide, rightSide;

    [SerializeField] public Image killerImage, victimImage;

    public Sprite[] characterImage;


    private Color[] HEX_COLOR = new Color[] {new Color(94f/255f,41f/255f,96f/255f), new Color(22f/255f,118f/255f,17f/255f)};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(PhotonView killer, PhotonView victim)
    {
        if (killer != null)
        {
            Debug.Log(killer.gameObject.GetComponent<IDamageable>().team.Code);
            Debug.Log(HEX_COLOR[(int)killer.gameObject.GetComponent<IDamageable>().team.Code]);
            leftSide.color = HEX_COLOR[(int)killer.gameObject.GetComponent<IDamageable>().team.Code];
            killerImage.GetComponent<Image>().sprite = characterImage[killer.gameObject.GetComponent<IDamageable>().characterID];
        }
        else
        {
            leftSide.color = Color.white;
            killerImage.sprite = null;
            killerImage.color = Color.white;
        }
        
        rightSide.color = HEX_COLOR[(int)victim.gameObject.GetComponent<IDamageable>().team.Code];

        
        victimImage.sprite = characterImage[victim.gameObject.GetComponent<IDamageable>().characterID];
        
        gameObject.SetActive(true);

        StartCoroutine(Undisplay());
    }

    IEnumerator Undisplay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
