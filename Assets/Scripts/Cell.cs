using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private TextMeshProUGUI _count;

    [SerializeField]
    private Image _image;



    public TextMeshProUGUI Text
    {
        get
        {
            return _text;
        }
    }

    public int Count
    {
        set
        {
            if (value > 0)
            {
                _count.text = $"{value}";

                _count.gameObject.SetActive(true);
            }
            else
            {
                _count.gameObject.SetActive(false);
            }
        }
    }

    public Sprite Icon
    {
        set
        {
            _image.gameObject.SetActive(value != null);

            _image.sprite = value;

            _image.preserveAspect = true;
        }
    }
}