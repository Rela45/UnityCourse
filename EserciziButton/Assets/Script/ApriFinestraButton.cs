using UnityEngine;
using UnityEngine.UI;

public class ApriFinestraButton : MonoBehaviour
{
    [SerializeField] private GameObject _apriFinestra;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => _apriFinestra.SetActive(true));            
    }
    
}
