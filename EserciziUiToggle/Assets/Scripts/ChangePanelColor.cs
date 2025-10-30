using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangePanelColor : MonoBehaviour
{
    //  [SerializeField] private TMP_Dropdown colorDropdown; // il menu a tendina
    [SerializeField] private Image panelImage;           // il pannello da colorare

    private void Start()
    {
        // GetComponent<TMP_Dropdown>.onValueChanged.AddListener(()=>ChangingColor);
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(ChangingColor);
    }

    public void ChangingColor(int index)
    {
        switch (index)
        {
            case 0:
                panelImage.color = Color.red;
                break;
            case 1:
                panelImage.color = Color.green;
                break;
            case 2:
                panelImage.color = Color.yellow;
                break;
            default:
                panelImage.color = Color.grey;
                break;
        }
    }
}
