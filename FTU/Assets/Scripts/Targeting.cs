using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    private GameObject entity;
    RaycastHit hit;
    // Start is called before the first frame update
    public void Start()
    {        
        //Debug.Log(player);
    }

    // Update is called once per frame
    public void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            try
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.collider.TryGetComponent(typeof(IDamageable), out Component component))
                    {
                        if (component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.minion ||
                            component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.voister ||
                            component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.player ||
                            component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.dieu ||
                            component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.golem)
                        {
                            if(hit.collider.GetComponent<IDamageable>().team != gameObject.GetComponent<IDamageable>().team)
                                entity.GetComponent<IDamageable>().Cible = hit.collider.gameObject;
                        }
                    }
                    else if (hit.collider.GetComponent<IDamageable>() == null)
                    {
                        entity.GetComponent<IDamageable>().Cible = null;
                    }
                }
            }
            catch(Exception ue)
            {
                //:)
            }
        }
    }
}
