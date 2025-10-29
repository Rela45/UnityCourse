using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class LogPulsante : MonoBehaviour
{

    [SerializeField] private string _nome;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Cliccato);
    }

    void Cliccato()
    {
        print(_nome);
    }
}
