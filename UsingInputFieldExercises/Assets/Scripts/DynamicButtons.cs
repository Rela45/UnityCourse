using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DynamicButtons : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;   // il prefab del bottone
    [SerializeField] private Transform contentParent;   // il "Content" della ScrollView

    void Start()
    {
        PopolaLista();
    }

    void PopolaLista()
    {
        for (int i = 1; i <= 20; i++)
        {
            // Crea un nuovo pulsante
            GameObject nuovoBottone = Instantiate(buttonPrefab, contentParent);

            // Cambia il testo
            TMP_Text testo = nuovoBottone.GetComponentInChildren<TMP_Text>();
            if (testo != null)
                testo.text = "Bottone " + i;

            // Aggiungi un listener al click
            Button btn = nuovoBottone.GetComponent<Button>();
            if (btn != null)
            {
                int index = i; // cattura la variabile per la lambda
                btn.onClick.AddListener(() => OnButtonClicked(index));
            }
        }
    }

    void OnButtonClicked(int index)
    {
        Debug.Log($"Hai cliccato il bottone {index}");
    }
}
