using UnityEngine;
using UnityEngine.UI;

public class DecrementButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(Cliccato);
        button.onClick.AddListener(PushDecrement);
    }

    void PushDecrement()
    {
        UiManager.Instance.Decrementa();
    }

    void Cliccato()
    {
        print("Hai cliccato il pulsante");
    }
}
