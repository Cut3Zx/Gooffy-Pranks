using UnityEngine;
using UnityEngine.EventSystems;

public class BaseObjectManager : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTransform;
    [System.NonSerialized]
    protected Canvas canvas;
    // ⚠️ Dùng NonSerialized để tránh trùng serialize ở class con
    [System.NonSerialized]
    protected Vector3 startPos;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (rectTransform != null)
            startPos = rectTransform.anchoredPosition;
        else
            startPos = transform.position;
    }

    // ---------------- Click ----------------
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        HandleClick();
    }

    protected void HandleClick()
    {
        Debug.Log($"{gameObject.name} was clicked!");
    }

    // ---------------- Drag ----------------
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        HandleDragging(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd();
    }

    protected void HandleDragStart()
    {
        Debug.Log($"{gameObject.name} drag started!");
    }

    protected void HandleDragging(PointerEventData eventData)
    {
        if (rectTransform != null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    protected void HandleDragEnd()
    {
        Debug.Log($"{gameObject.name} drag ended!");
    }

    // ---------------- Reset Position ----------------
    protected void ResetPosition()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition = startPos;
        else
            transform.position = startPos;
    }
}
