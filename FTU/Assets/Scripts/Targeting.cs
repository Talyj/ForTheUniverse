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
                if (hit.collider.GetComponent<IDamageable>() != null)
                {
                    if (hit.collider.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.minion || 
                        hit.collider.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.voister || 
                        hit.collider.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.joueur || 
                        hit.collider.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.dieu ||
                        hit.collider.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.golem)
                    {
                        player.GetComponent<Dps1>().Cible = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.GetComponent<IDamageable>() == null)
                {
                    player.GetComponent<IDamageable>().Cible = null;
                }
            }
        }
    }
}
