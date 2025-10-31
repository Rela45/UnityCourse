using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PushButton);
    }

    void PushButton()
    {
        UIMANAGER1.Instance.ButtonPressed();
        print("Inviato Correttamente");
    }
    
}
