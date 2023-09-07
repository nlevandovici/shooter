using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Vector3 _cameraOffset;

    [SerializeField]
    private float _cameraSpeed = 5f;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _rotationSpeed = 30f;

    [SerializeField]
    private float _moveSpeed = 3f;

    [SerializeField]
    private float _runSpeed = 7f;

    [SerializeField]
    private float _gravity = -9.81f;

    [SerializeField]
    private float _health = 100f;

    [SerializeField]
    private float _maxHealth = 100f;

    [SerializeField]
    private float _jump = 9f;

    private float _yVelocity;

    private bool _grounded = false;



    public event Func<Item, Item> OnTryCollectItem;

    public event Action<Enemy, Vector3> OnHitEnemy;

    public event Action OnGameOver;



    public float Health
    {
        get
        {
            return _health;
        }
    }

    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
    }



    private void Awake()
    {
        _camera = Camera.main;

        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        _health = _maxHealth;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;

                Cursor.visible = true;
            }
            else if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }
        }

        if (_health > 0f)
        {
            if (_grounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _yVelocity = Mathf.Sqrt(_jump);
                }
                else if (_yVelocity < _gravity)
                {
                    _yVelocity = _gravity;
                }
            }

            Vector3 move = transform.forward * Input.GetAxis("Vertical") +
                transform.right * Input.GetAxis("Horizontal") + transform.up * _yVelocity;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _characterController.Move(move * _runSpeed * Time.deltaTime);
            }
            else _characterController.Move(move * _moveSpeed * Time.deltaTime);


            float x = Input.GetAxis("Mouse X");

            float y = Input.GetAxis("Mouse Y");

            transform.eulerAngles += new Vector3(0f, _rotationSpeed * x, 0f);

            _camera.transform.localEulerAngles = new Vector3(Mathf.Clamp(_camera.transform.localEulerAngles.x + _rotationSpeed * y * -1f, 5f, 45f), 0f, 0f);

            _yVelocity += _gravity * Time.deltaTime;


            if (Input.GetMouseButton(0))
            {
                Enemy enemy = HitEnemy(out Vector3 point);

                OnHitEnemy.Invoke(enemy, point);
            }

            if (Input.GetMouseButton(1))
            {
                _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition,
                    _cameraOffset * 0.7f, _cameraSpeed * Time.deltaTime);

                Enemy enemy = HitEnemy(out Vector3 point);

                if (enemy != null)
                {
                    enemy.ShowUI();
                }
            }
            else
            {
                _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition,
                    _cameraOffset, _cameraSpeed * Time.deltaTime);
            }
        }
    }



    public void TakeDamage(float damage)
    {
        if (_health > 0f)
        {
            _health -= damage;

            if (_health <= 0f)
            {
                _health = 0f;

                OnGameOver.Invoke();
            }
        }
    }



    private Enemy HitEnemy(out Vector3 point)
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500))
        {
            Debug.DrawLine(ray.origin, hit.point);
        }

        if (hit.transform != null && hit.transform.tag == "Enemy")
        {
            Debug.Log("Hit");

            point = hit.point;

            return hit.transform.GetComponent<Enemy>();
        }
        else
        {
            point = Vector3.zero;

            return null;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            FloatingItem floatingItem = other.gameObject.GetComponent<FloatingItem>();

            Item item = OnTryCollectItem.Invoke(floatingItem.Item);

            if (item.Type == EItem.none)
            {
                Destroy(other.gameObject);
            }
            else
            {
                floatingItem.Item = item;
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _grounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _grounded = false;
        }
    }
}