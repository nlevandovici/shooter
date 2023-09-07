using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    private int _count = 4;

    [SerializeField]
    private int _selectedId = 0;

    [SerializeField]
    private Item[] _items = new Item[0];

    [SerializeField]
    private Cell[] _cells = new Cell[0];

    [SerializeField]
    private Cell _cellPrefab;

    [SerializeField]
    private RectTransform _inventoryPanel;

    [SerializeField]
    private float _reloading = 0f;

    [SerializeField]
    private float _reloadingTime = 0f;

    [SerializeField]
    private GameObject _reloadingPanel;

    [SerializeField]
    private Slider _reloadingSlider;



    public event Action<Item> OnDropItem;



    public int SelectedID
    {
        get
        {
            return _selectedId;
        }
        private set
        {
            SelectCell(value);
        }
    }



    private void Update()
    { 
        if (_reloading <= 0f)
        {
            _reloading = 0f;

            _reloadingPanel.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Alpha1) && _count >= 1)
            {
                SelectedID = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && _count >= 2)
            {
                SelectedID = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && _count >= 3)
            {
                SelectedID = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && _count >= 4)
            {
                SelectedID = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && _count >= 5)
            {
                SelectedID = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && _count >= 6)
            {
                SelectedID = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && _count >= 7)
            {
                SelectedID = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) && _count >= 8)
            {
                SelectedID = 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9) && _count >= 9)
            {
                SelectedID = 8;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0) && _count >= 10)
            {
                SelectedID = 9;
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                Item item = _items[SelectedID];

                if (item.Type != EItem.none)
                {
                    _items[SelectedID] = new Item();

                    OnDropItem(item);

                    SetUpCells();
                }
            }
        }
        else
        {
            _reloading -= Time.deltaTime;

            _reloadingPanel.SetActive(true);

            _reloadingSlider.value = _reloading / _reloadingTime;
        }
    }



    public Item TryCollect(Item item)
    {
        if (item.Type == EItem.gun)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Type == EItem.none)
                {
                    _items[i] = item;

                    SetUpCells();

                    return new Item();
                }
            }
        }
        else if (item.Type == EItem.bullets)
        {
            int count = item.Count;

            for (int i = 0; i < _items.Length; i++)
            {
                Bullet bullet = _items[i].Object as Bullet;

                if (_items[i].Type == EItem.bullets && bullet.Name == (item.Object as Bullet).Name)
                {
                    _items[i].Count += count;

                    SetUpCells();

                    return new Item();
                }
            }

            for (int i = 0; i < _items.Length; i++)
            {
                Bullet bullet = _items[i].Object as Bullet;

                if (_items[i].Type == EItem.none)
                {
                    _items[i] = item;

                    SetUpCells();

                    return new Item();
                }
            }
        }

        SetUpCells();

        return item;
    }

    public bool IsGunSelected(out Item item, out Gun gun)
    {
        if (_items[_selectedId].Type == EItem.gun)
        {
            gun = _items[_selectedId].Object as Gun;

            item = _items[_selectedId];

            return true;
        }

        gun = null;

        item = null;

        return false;
    }

    public bool TryReloadGun(Item item, Gun gun)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].Type == EItem.bullets && (_items[i].Object as Bullet).Name == gun.BulletType.Name)
            {
                if (_items[i].Count > gun.MagazineCapacity)
                {
                    _items[i].Count -= gun.MagazineCapacity;

                    item.Count += gun.MagazineCapacity;
                } 
                else
                {
                    item.Count += _items[i].Count;

                    _items[i] = new Item();
                }

                return true;
            }
        }

        return false;
    }

    public void SetReloading(float time)
    {
        _reloading = time;

        _reloadingTime = time;
    }



    public void OnValidateGUI()
    {
        if (_items.Length != _count)
        {
            Item[] items = _items;

            _items = new Item[_count];

            for (int i = 0; i < _count && i < items.Length; i++)
            {
                _items[i] = items[i];
            }

            for(int i = items.Length; i < _count; i++)
            {
                _items[i] = new Item();
            }
        }

        SetUpCells();
    }



    public void SetUpCells()
    {
        if (_cells.Length != _count)
        {
            _inventoryPanel.sizeDelta = new Vector2(100f * _count + 12f * (_count + 1), _inventoryPanel.sizeDelta.y);

            for (int i = 0; i < _cells.Length; i++)
            {
                DestroyImmediate(_cells[i].gameObject);
            }

            _cells = new Cell[_count];

            for (int i = 0; i < _count; i++)
            {
                Vector3 position = new Vector3(-112f * (_count / 2) + i * 112f - 56f * (_count % 2 - 1), 0f, 0f);

                _cells[i] = Instantiate(_cellPrefab, position, Quaternion.identity, _inventoryPanel);

                _cells[i].GetComponent<RectTransform>().anchoredPosition = position;

                _cells[i].Text.text = $"{(i + 1) % 10}";
            }

            SelectCell(0);
        }

        for(int i = 0; i < _cells.Length; i++)
        {
            if (_items[i].Type == EItem.none)
            {
                _cells[i].Count = 0;

                _cells[i].Icon = null;
            }
            else if (_items[i].Type == EItem.gun)
            {
                _cells[i].Icon = (_items[i].Object as Gun).Icon;

                _cells[i].Count = _items[i].Count;
            }
            else if (_items[i].Type == EItem.bullets)
            {
                _cells[i].Icon = (_items[i].Object as Bullet).Icon;

                _cells[i].Count = _items[i].Count;
            }
        }
    }

    private void SelectCell(int cellId)
    {
        if (_selectedId >= 0 && _selectedId < _cells.Length)
            _cells[_selectedId].GetComponent<Image>().color = Color.white;

        _cells[cellId].GetComponent<Image>().color = Color.green;

        _selectedId = cellId;
    }
}