using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private Inventory _target;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(_target == null)
        {
            _target = target.GetComponent<Inventory>();
        }

        _target.OnValidateGUI();
    }
}