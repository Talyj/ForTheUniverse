using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    RaycastHit hit;
    // Start is called before the first frame update
    public void Start()
    {
        //player = gameObject;
        //Debug.Log(player);
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<Targetable>() != null)
                {
                    if (hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion || 
                        hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.voister || 
                        hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.joueur || 
                        hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.dieu ||
                        hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.golem)
                    {
                        player.GetComponent<Dps1>().Cible = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.GetComponent<Targetable>() == null)
                {
                    player.GetComponent<Dps1>().Cible = null;
                }
            }
        }
    }
}
