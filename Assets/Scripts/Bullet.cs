using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/Bullet", order = 2)]
public class Bullet : ScriptableObject
{
    public string Name;

    public Sprite Icon;

    public GameObject Prefab;
}
