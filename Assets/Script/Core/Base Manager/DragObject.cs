using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : BaseObjectManager
{
    protected override void Awake()
    {
        base.Awake();
        // Không cần khai báo lại rectTransform vì Base đã có sẵn
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} bắt đầu kéo!");
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Dùng rectTransform kế thừa từ BaseObjectManager
        if (rectTransform != null && canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} kéo xong!");
    }
}
