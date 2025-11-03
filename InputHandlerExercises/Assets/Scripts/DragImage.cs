using System.Net.Sockets;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragImage : MonoBehaviour,
IBeginDragHandler,
IDragHandler,
IEndDragHandler,
ISelectHandler,
IDeselectHandler,
IPointerDownHandler
{

    [SerializeField] private GameObject _myPanel;
    private RectTransform rect;
    private RectTransform panelRect;


    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.gray;
        print("sto muovendo il mio oggetto");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform panelRect = _myPanel.GetComponent<RectTransform>();
        RectTransform rect = GetComponent<RectTransform>();

        Vector2 mouseDelta = eventData.delta; // delta in pixel

        // Calcola la nuova posizione
        Vector2 newPos = rect.anchoredPosition + mouseDelta;

        // Limiti basati sul panel
        Vector2 panelSize = panelRect.rect.size;
        Vector2 imageSize = rect.rect.size;

        float halfPanelX = panelSize.x / 2;
        float halfPanelY = panelSize.y / 2;
        float halfImageX = imageSize.x / 2;
        float halfImageY = imageSize.y / 2;

        // Clamp per non uscire dal panel
        newPos.x = Mathf.Clamp(newPos.x, -halfPanelX + halfImageX, halfPanelX - halfImageX);
        newPos.y = Mathf.Clamp(newPos.y, -halfPanelY + halfImageY, halfPanelY - halfImageY);

        // Applica la nuova posizione
        rect.anchoredPosition = newPos;


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
        print("ho smesso di muovere il mio oggetto");
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        print("sto cliccando l'immagine");
    }

    public void OnSelect(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }

}
