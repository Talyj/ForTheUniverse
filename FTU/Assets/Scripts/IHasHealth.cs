using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHasHealth : MonoBehaviour
{
    private float _health;

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float healthToSet)
    {
        _health = healthToSet;
    }

    public bool IsDead()
    {
        if(_health <= 0)
        {
            return true;
        }
        return false;
    }

    public void TakeDamages(float dmg)
    {
        _health = GetHealth() - dmg;
    }
}
