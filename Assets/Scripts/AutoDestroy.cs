using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float _destroyAfter = 4f;



    private void Start()
    {
        Destroy(gameObject, _destroyAfter);
    }
}
