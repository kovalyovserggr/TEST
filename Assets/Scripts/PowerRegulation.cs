using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PowerRegulation : MonoBehaviour
{    
    [SerializeField] TextMeshProUGUI _textValue;
    [SerializeField] Slider _slider;

    public static event Action<float> ChangePower;

    public void ValueChange()
    {
        if ((_slider == null) || (_textValue == null))
        {
            return;
        }

        _textValue.text = (_slider.value).ToString();

        ChangePower?.Invoke(_slider.value);
    }
}
