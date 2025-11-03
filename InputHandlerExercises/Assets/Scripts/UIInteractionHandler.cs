using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIInteractionHandler : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEngine.UI.Image _myImage;


    public void OnPointerEnter(PointerEventData eventData)
    {
        _myImage.color = Color.red;
        print("Sono entrato nell'oggetto");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _myImage.color = Color.black;
        print("Sono uscito dall'oggetto");
    }

    
}
