using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingsBehaviour : BasicAIMovement
{
    public int numberOfFollower;
    public int followersMax = 5;
    //Setup in specific class
    protected string followersTag;
    [SerializeField] protected GameObject voister;
    protected float cpt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BaseSetupKing()
    {
        numberOfFollower = 0;
        team.Code = 2;
    }

    public void BaseBehaviourKings()
    {
        HealthBehaviour();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void CheckProtector()
    {
        var targs = Physics.OverlapSphere(transform.position, 20).Where(w => w.gameObject.CompareTag(followersTag));
        Debug.Log(numberOfFollower);
        numberOfFollower = targs.Count() < numberOfFollower ? targs.Count() : numberOfFollower;
    }

    protected void SpawnVoisters(GameObject voister, KingsBehaviour king)
    {
        var x = Random.Range(-150, 150);
        var z = Random.Range(-50, 50);
        
        var voisterTemp = PhotonNetwork.Instantiate(voister.name, new Vector3(x, king.transform.position.y, z), Quaternion.identity);
        voisterTemp.GetComponent<VoisterBehaviour>().kingVoisters = king;
    }
}
