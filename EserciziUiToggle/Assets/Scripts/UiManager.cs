using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private UiManager _instance;
    public UiManager Instance { get { return _instance; } }

    [SerializeField] private GameObject _Panel;

    [SerializeField] private Toggle _apriFinestra;

    [SerializeField] private GameObject _gatoImage;

    [SerializeField] private TextMeshProUGUI _toToggleText;

    [SerializeField] private Button _toToggleButton;

    [SerializeField] private Toggle _imageToggle;
    [SerializeField] private Toggle _textToggle;
    [SerializeField] private Toggle _toggleButton;

    [SerializeField] private TextMeshProUGUI _forzaNapoli;
    [SerializeField] private List<Toggle> _togglesReset;

    [SerializeField] private Light _light;

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
        _apriFinestra.onValueChanged.AddListener(OpenPanel);
        _imageToggle.onValueChanged.AddListener(OpenImage);
        _textToggle.onValueChanged.AddListener(OpenText);
        _toggleButton.onValueChanged.AddListener(OpenButton);
        _toToggleButton.onClick.AddListener(ButtonClicked);
    }


    void OpenPanel(bool isOn)
    {
        
        if (isOn)
        {
            _Panel.SetActive(true);
            _light.intensity = 2;
        }
        else
        {
            _Panel.SetActive(false);
        }
    }

    void OpenImage(bool isOn)
    {
        if (isOn)
        {
            _gatoImage.SetActive(true);
        }
        else
        {
            _gatoImage.SetActive(false);
        }
    }

    void OpenText(bool isOn)
    {
        if (isOn)
        {
            _toToggleText.gameObject.SetActive(true);
        }
        else
        {
            _toToggleText.gameObject.SetActive(false);
        }
    }

    void OpenButton(bool isOn)
    {
        if (isOn)
        {
            _toToggleButton.gameObject.SetActive(true);
        }
        else
        {
            _toToggleButton.gameObject.SetActive(false);
        }
    }

    void ButtonClicked()
    {
        _Panel.SetActive(false);
        _gatoImage.SetActive(false);
        _toToggleText.gameObject.SetActive(false);
        _toToggleButton.gameObject.SetActive(false);
        foreach (Toggle t in _togglesReset)
        {
            t.isOn = false;
        }

        _forzaNapoli.gameObject.SetActive(true);
    }
}
