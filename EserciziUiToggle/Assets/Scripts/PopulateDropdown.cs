using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class PopulateDropdown : MonoBehaviour
{
    [SerializeField] private GameObject _maradona;
    [SerializeField] private GameObject _spidermeme;
    [SerializeField] private GameObject _gatoTerorista;

    public List<GameObject> images;
    void Start()
    {
        List<string> list = new List<string>();
        list.Add("Spidermeme");
        list.Add("Maradona");
        list.Add("gato terorista");
        images.Add(_spidermeme);
        images.Add(_maradona);
        images.Add(_gatoTerorista);
        AllDisabled();
        InsertIntoDD(list);

        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(Choice);
    }


    void InsertIntoDD(List<string> options)    //mi permette di inserire dinamicamente elementi all'interno del mio dropdown
    {
        GetComponent<TMP_Dropdown>().ClearOptions();
        GetComponent<TMP_Dropdown>().AddOptions(options);
    }

    void AllDisabled()
    {
        foreach(GameObject s in images)
        {
            s.SetActive(false);
        }
    }
    
    void Choice(int index)
    {
        AllDisabled();
        if(index >= 0 && index < images.Count)
        {
            images[index].SetActive(true);
        }
        // switch (index)
        // {
        //     case 0:
                
        //         _spidermeme.SetActive(true);
        //         break;
        //     case 1:
        //         _maradona.SetActive(true);
        //         break;
        //     case 2:
        //         _gatoTerorista.SetActive(true);
        //         break;
        // }
    }
}
