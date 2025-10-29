using UnityEngine;
using UnityEngine.UI;

public class IncrementButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(Cliccato);
        button.onClick.AddListener(PushIncrement);
    }

    void PushIncrement()
    {
        UiManager.Instance.Incrementa();
    }

    void Cliccato()
    {
        print("Hai cliccato il pulsante");
    }
}
