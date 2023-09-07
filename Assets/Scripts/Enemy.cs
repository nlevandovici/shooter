using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _showUITimer = 0f;

    [SerializeField]
    private EnemyUIPanel _ui;

    [SerializeField]
    private float _health = 100f;

    private float _viewRadius = 9f;

    [SerializeField]
    private float _maxHealth = 100f;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private bool _run = false;

    [SerializeField]
    private Vector2 _damage = new Vector2(9f, 17f);

    [SerializeField]
    private float _hitDelay = 1.3f;

    [SerializeField]
    private float _lastHit = 0f;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private bool _found = false;



    public event Action OnDestroy;

    public event Action OnKick;


    private void Awake()
    {
        _health = _maxHealth;
    }

    private void Start()
    {
        _ui.Hide();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }



    private void Update()
    {
        if(_showUITimer > 0f)
        {
            _showUITimer -= Time.deltaTime;
        }
        else
        {
            _ui.Hide();
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance < 1.5f)
        {
            _animator.SetBool("Walk", false);

            if(Time.time > _lastHit + _hitDelay)
            {
                _lastHit = Time.time;

                _player.TakeDamage(UnityEngine.Random.Range(_damage.x, _damage.y));

                _found = true;

                OnKick.Invoke();

                _animator.SetTrigger("Attack");
            }
        }
        else if (distance < _viewRadius || _health < _maxHealth || _found)
        {
            transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.transform.position.x,
                transform.position.y, _player.transform.position.z), _moveSpeed * Time.deltaTime);

            _animator.SetBool("Walk", true);
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

                _animator.SetBool("Death", true);

                OnDestroy.Invoke();

                Destroy(gameObject, 1f);
            }
        }

        _ui.HealthBar = _health / _maxHealth;
    }



    public void ShowUI()
    {
        ShowUI(3f);
    }

    private void ShowUI(float time)
    {
        _showUITimer = time;

        _ui.Show();
    }
}
