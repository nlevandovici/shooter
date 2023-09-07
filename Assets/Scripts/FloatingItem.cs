using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 20f;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private Item _item;



    public Item Item
    {
        get
        {
            return _item;
        }

        set
        {
            _item = value;
        }
    }



    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position +
            new Vector3(0f, Mathf.Sin(Time.time), 0f), _moveSpeed * Time.deltaTime);

        transform.Rotate(new Vector3(0f, _rotationSpeed * Time.deltaTime, 0f));
    }
}
