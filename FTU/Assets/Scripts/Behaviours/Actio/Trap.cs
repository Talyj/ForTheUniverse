using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public IDamageable.DamageType typeDegats;
    public float degats;
    public Skills skills;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        
            
        if (col.gameObject.GetComponent<IDamageable>())
        {
            float DmgPerHeal;
            float heal = col.gameObject.GetComponent<IDamageable>().GetHealth();
            DmgPerHeal = heal / 4;
            degats = skills.Damage + DmgPerHeal;
            Debug.Log("trap " + degats);
            col.gameObject.GetComponent<IDamageable>().TakeDamage(degats, typeDegats);
        }
        
    }


}
