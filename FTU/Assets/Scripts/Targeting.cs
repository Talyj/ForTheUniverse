using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    private GameObject entity;
    [SerializeField]
    LayerMask layerToIgnore;
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity,layerToIgnore))
            {
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward * Mathf.Infinity, Color.red);
                if (hit.collider.TryGetComponent(typeof(IDamageable), out Component component))
                {
                    if (component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.minion ||
                        component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.voister ||
                        component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.player ||
                        component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.dieu ||
                        component.GetComponent<IDamageable>().GetEnemyType()== IDamageable.EnemyType.golem)
                    {
                        if(hit.collider.GetComponent<IDamageable>().team.Code != gameObject.GetComponent<IDamageable>().team.Code) entity.GetComponent<IDamageable>().Cible = hit.collider.gameObject;
                        return;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            this.gameObject.GetComponent<IDamageable>().Cible = null;
        }
    }
}
