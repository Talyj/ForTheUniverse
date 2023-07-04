using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Item",menuName ="Items/new item")]
public class ItemStats : ScriptableObject
{
    public string nameItem;
    public float price;
    [Tooltip("Augmente la vie max")]
    public float health;
    [Tooltip("Augmente la mana max")]
    public float mana;
    [Tooltip("Augmente la resistance phys")]
    public float resPhys;
    [Tooltip("Augmente la resistance mag")]
    public float resMag;
    [Tooltip("Augmente les degats phys")]
    public float dmgPhys;
    [Tooltip("Augmente les degats mag")]
    public float dmgMag;
    [Tooltip("Augmente la vitesse d'attaque")]
    public float attackSpeed;
    [Tooltip("Image")]
    public Sprite img;
    [Tooltip("Rarete")]
    public ItemRarete rarete;

    public virtual IEnumerator OnUse()
    {
        Debug.Log("No effect : " + nameItem);
        yield return 0;
    }
}

public enum ItemRarete{
    Consommable,
    Objet,
    Talisman,
    Relique
}
