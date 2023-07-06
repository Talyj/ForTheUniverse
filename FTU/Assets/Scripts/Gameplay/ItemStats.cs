using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/new item")]
public class ItemStats : ScriptableObject
{
    public int ID;
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
    //[Tooltip("Quantité de l'objet")]
    //public int qty;
    [Tooltip("Rarete")]
    public ItemRarete rarete;
    [Tooltip("ID passif (0 si aucun)")]
    public int idPassif;
}

public enum ItemRarete
{
    Consommable,
    Objet,
    Talisman,
    Relique
}
