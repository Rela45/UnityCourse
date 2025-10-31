
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIMANAGER1 : MonoBehaviour
{
    private static UIMANAGER1 _instance;
    public static UIMANAGER1 Instance { get { return _instance; } }

    [SerializeField] private TextMeshProUGUI _toPrintText;
    [SerializeField] private Button _toPressButton;
    
    private int numero;
    private Color coloreErrore = new Color(1f, 0.4f, 0.4f);
    private Color coloreNormale = Color.white;
    [SerializeField] private TMP_InputField _input;

    [SerializeField] private TMP_InputField[] _inputFields;
    void Awake()
    {
        print("chiamata da awake");
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
        print("sto chiamando start da UIMANAGER");
        _input.onValueChanged.AddListener(VerifyNum);
    }

    void VerifyNum(string s)
    {
        
        s = _input.text;
        Debug.Log("sto entrando nella funzione");
        if (int.TryParse(s, out numero))
        {
            print("tryParse riuscito");
            if (numero > 100)
            {
                _toPrintText.text = "il numero è maggiore di 100";
            }
            else _toPrintText.text = "il numero è minore o uguale a 100";
        }
    }

    public void ButtonPressed()
    {
        // bool allCompiled = true;
        foreach (TMP_InputField s in _inputFields)
        {
            if (string.IsNullOrEmpty(s.text))
            {
                s.image.color = coloreErrore;
                // allCompiled = false;
            }
            else
            {
                s.image.color = coloreNormale;
            }
        }
    }

}
