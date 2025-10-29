using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static UiManager _instance;
    public static UiManager Instance { get { return _instance; } }

    [SerializeField] private TextMeshProUGUI _text;

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
