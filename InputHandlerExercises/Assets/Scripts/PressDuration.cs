using UnityEngine;
using UnityEngine.EventSystems;
public class PressDuration : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    private float _pressStart;

    private float _sogliaClick = 0.5f;


    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _pressStart = Time.time;
        print("Pointer pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float durata = Time.time - _pressStart;

        if (durata >= _sogliaClick)
        {
            print($"pressione lunga durata : {durata}");
        }
        else print($"pressione corta durata : {durata}");
    }

}
