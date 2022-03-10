using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName ="Skills",menuName ="My Game/Skills")]


public class Skills : ScriptableObject
{
    public string Name;
    public float Cost;
    public float CastTime;
    public float Cooldown;
    public bool isCooldown;
    public TypeSkills type;
    public float DamagePhy;
    public float DamageMag;
    public Image image;


}
public enum TypeSkills
{
    Normal,
    SkillShot,
    Buff,
    Debuff
}