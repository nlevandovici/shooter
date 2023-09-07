using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public EItem Type = EItem.none;

    public ScriptableObject Object;

    public int Count = 0;



    public Item(EItem type, ScriptableObject @object, int count)
    {
        Type = type;

        Object = @object;
        
        Count = count;
    }

    public Item() : this(EItem.none, null, 0)
    {

    }
}



public enum EItem
{
    none, gun, bullets
}