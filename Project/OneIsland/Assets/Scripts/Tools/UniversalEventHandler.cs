using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UniversalEventHandler : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    [System.Serializable]
    public class MyEvent : UnityEvent<PointerEventData> { }

    public delegate void PointerEventDelegate(PointerEventData eventData);

    /// <summary>
    /// 点击事件
    /// </summary>
    public PointerEventDelegate OnLeftClickEvent;        // 左键点击
    public PointerEventDelegate OnRightClickEvent;       // 右键点击
    public PointerEventDelegate OnMiddleClickEvent;     // 中键点击
    public PointerEventDelegate OnPointerDownEvent;     
    public PointerEventDelegate OnPointerUpEvent;

    /// <summary>
    /// 拖拽事件
    /// </summary>
    public PointerEventDelegate OnBeginDragEvent;
    public PointerEventDelegate OnDragEvent;
    public PointerEventDelegate OnEndDragEvent;

    // 点击事件处理
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OnLeftClickEvent?.Invoke(eventData);
                break;
            case PointerEventData.InputButton.Right:
                OnRightClickEvent?.Invoke(eventData);
                break;
            case PointerEventData.InputButton.Middle:
                OnMiddleClickEvent?.Invoke(eventData);
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke(eventData);
    }

    // 拖拽事件
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(eventData);
    }
}