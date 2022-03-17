using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject player;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<Targetable>() != null)
                {
                    if (hit.collider.GetComponent<Targetable>().enemytype == Targetable.EnemyType.minion)
                    {
                        player.GetComponent<PlayerStats>().Cible = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.GetComponent<Targetable>() == null)
                {
                    player.GetComponent<PlayerStats>().Cible = null;
                }
            }
        }
    }
}
