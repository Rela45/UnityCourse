using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertName : MonoBehaviour
{
    [SerializeField] private Button _toPressButton;
    [SerializeField] private TextMeshProUGUI _toStampText;

    private string _tempName; //la utilizzo solo se il file lo vado a mettere in un obj diverso dall'input field tipo uno UImanager ma poi dovrei modificare tutto
    void Start()
    {
        GetComponent<TMP_InputField>().onValueChanged.AddListener((string s) => PrintOnScreen()); //lambda altrimenti non mi funziona niente
        _toPressButton.onClick.AddListener(Send);
    }

    void PrintOnScreen()        //stampo la mia stringa sul text field sull'interfaccia
    {
        _toStampText.text = "Username: " + GetComponent<TMP_InputField>().text;
    }
    void Send()
    {
        _toStampText.gameObject.SetActive(true);
    }
}
