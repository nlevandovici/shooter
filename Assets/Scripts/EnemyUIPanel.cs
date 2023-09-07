using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIPanel : UIPanel
{
    private Camera _camera;

    private Canvas _canvas;

    [SerializeField]
    private Slider _healthBar;



    public float HealthBar
    {
        set
        {
            _healthBar.value = value;
        }
    }



    private void Awake()
    {
        _camera = Camera.main;

        _canvas = GetComponent<Canvas>(); 
        
        _canvas.worldCamera = _camera;
    }



    private void Update()
    {
        transform.forward = (transform.position - _camera.transform.position).normalized;
    }
}