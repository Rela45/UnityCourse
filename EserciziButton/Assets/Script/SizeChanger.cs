using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SizeChanger : MonoBehaviour
{
    [SerializeField] private Button _reset;
    [SerializeField] private Transform targetObject;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        _slider.onValueChanged.AddListener(ChangeSize);
        _reset.onClick.AddListener(ResetSize);
    }

    void ChangeSize(float value)
    {
        targetObject.localScale = Vector3.one * value;
        _text.text = $"{value:F2}";
    }
    
    void ResetSize()
    {
        targetObject.localScale = Vector3.one;
        _text.text = $"1.00";
    }
    
    
}
