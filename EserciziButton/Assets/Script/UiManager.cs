using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private static UiManager _instance;
    public static UiManager Instance { get { return _instance; } }

    [SerializeField] private Slider _slider;

    [SerializeField] private Slider _luminosit;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _volume;
    public float volume = 0;
    public int increment = 0;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _slider.onValueChanged.AddListener(ModifyVolume);
    }

    void ModifyVolume(float value)
    {
        volume = value;
        _volume.text = $"{volume:F2} %";
    }
    public void Incrementa()
    {
        increment++;
        _text.text = $"{increment}";
    }

    public void Decrementa()
    {
        increment--;
        _text.text = $"{increment}";
    }
}
