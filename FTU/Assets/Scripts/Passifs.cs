using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Passif", menuName = "My Game/Passifs")]
public class Passifs : ScriptableObject
{
    public string Name;
    public float Cooldown;
    public float Bonus;
    public float Malus;
    public bool isCooldown;
    public Image image;
}
