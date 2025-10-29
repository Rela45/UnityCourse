using UnityEngine;
using UnityEngine.UI;

public class ChiudiFinestra : MonoBehaviour
{
    [SerializeField] private GameObject _chiudiFinestra;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => _chiudiFinestra.SetActive(false));            
    }
}
