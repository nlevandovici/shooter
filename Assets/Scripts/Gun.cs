using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 1)]
public class Gun : ScriptableObject
{
    public string Name;

    public float Damage;

    public float FireDelay;

    public int MagazineCapacity;

    public float ReloadTime;

    public Sprite Icon;

    public GameObject Prefab;

    public AudioClip[] fire;


    public Bullet BulletType;
}