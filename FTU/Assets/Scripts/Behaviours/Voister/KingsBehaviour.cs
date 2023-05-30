using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingsBehaviour : BasicAIMovement
{
    public int numberOfFollower;
    //Setup in specific class
    protected string followersTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BaseSetupKing()
    {
        numberOfFollower = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckProtector();
    }

    protected void CheckProtector()
    {
        var targs = Physics.OverlapSphere(transform.position, 20).Where(w => w.gameObject.CompareTag(followersTag));
        foreach(var fol in targs)
        {
            numberOfFollower++;
        }
    }
}
