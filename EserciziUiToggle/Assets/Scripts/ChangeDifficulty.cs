using TMPro;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _choice;
    void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(MakeAChoice);
    }

    void MakeAChoice(int index)
    {
        switch (index)
        {
            case 0:
                _choice.text = "Hai scelto la modalita' facile";
                break;
            case 1:
                _choice.text = "Hai scelto la modalita' Normale";
                break;
            case 2:
                _choice.text = "Hai scelto la modalita' difficile";
                break;
            default:
                _choice.text = "Forza Napoli";
                break;
        }
    }
    
}
