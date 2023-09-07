using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelSoundManager _soundManager;

    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private Gun[] _guns;

    [SerializeField]
    private Bullet[] _bullets;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _lastHitTime = 0f;

    [SerializeField]
    private GameObject _hitEffect;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private Enemy _enemy;

    [SerializeField]
    private int _kills = 0;

    [SerializeField]
    private Slider _healthBar;

    [SerializeField]
    private GameObject _losePanel;

    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private TextMeshProUGUI _winsLoses;

    [SerializeField]
    private Button _playAgain;



    private void Awake()
    {
        _playAgain.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }



    private void Start()
    {
        SaveLoadManager.Load();

        _player.OnTryCollectItem += (item) =>
        {
            return _inventory.TryCollect(item);
        };

        _player.OnHitEnemy += (enemy, point) =>
        {
            Item item;

            Gun gun;

            if (_inventory.IsGunSelected(out item, out gun))
            {
                if (Time.time > _lastHitTime + gun.FireDelay)
                {
                    if (item.Count > 0)
                    {
                        if (enemy != null)
                        {
                            enemy.TakeDamage(gun.Damage);

                            Instantiate(_hitEffect, point, Quaternion.identity);
                        }

                        _soundManager.PlaySfx(gun.fire[Random.Range(0, gun.fire.Length)]);

                        _lastHitTime = Time.time;

                        item.Count--;
                    }
                    else if (_inventory.TryReloadGun(item, gun))
                    {
                        _soundManager.PlaySfx(LevelSoundManager.ESFX.reloading);

                        _lastHitTime = Time.time + gun.ReloadTime;

                        _inventory.SetReloading(gun.ReloadTime);
                    }

                    _inventory.SetUpCells();
                }
            }
        };

        _inventory.OnDropItem += (item) =>
        {
            Vector3 pos = _player.transform.position + _player.transform.forward * 2f;
            
            if (item.Type == EItem.gun)
            {
                Instantiate((item.Object as Gun).Prefab, new Vector3(pos.x, 1f,
                    pos.z), Quaternion.identity).GetComponent<FloatingItem>().Item = item;
            }
            else if(item.Type == EItem.bullets)
            {
                Instantiate((item.Object as Bullet).Prefab, new Vector3(pos.x, 1f,
                    pos.z), Quaternion.identity).GetComponent<FloatingItem>().Item = item;
            }
        };

        _player.OnGameOver += () =>
        {
            //lose
            SaveLoadManager.AddLose();

            SaveLoadManager.Save();

            _losePanel.SetActive(true);

            _winsLoses.gameObject.SetActive(true);

            _winsLoses.text = $"Wins: {SaveLoadManager.WinCount} | Loses: {SaveLoadManager.LoseCount}";

            _playAgain.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;
        };

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Enemy enemy = Instantiate(_enemy, _spawnPoints[i].position, Quaternion.identity);

            enemy.OnDestroy += () =>
            {
                _kills++;
            };

            enemy.OnKick += () =>
            {
                _soundManager.PlaySfx(LevelSoundManager.ESFX.kick);
            };
        }
    }

    private void Update()
    {
        if(_kills >= _spawnPoints.Length)
        {
            //win

            SaveLoadManager.AddWin();

            SaveLoadManager.Save();

            _winPanel.SetActive(true);

            _kills = 0;

            _winsLoses.gameObject.SetActive(true);

            _winsLoses.text = $"Wins: {SaveLoadManager.WinCount} | Loses: {SaveLoadManager.LoseCount}";

            _playAgain.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;
        }

        _healthBar.value = _player.Health / _player.MaxHealth;
    }
}