using UnityEngine;
using UnityEngine.EventSystems;

public class HideAndShowUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Đối tượng khi chạm sẽ ẩn đi")]
    public GameObject objectToHide;

    [Header("Đối tượng sẽ hiện ra khi chạm")]
    public GameObject objectToShow;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // để kéo luôn nằm trên cùng
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kiểm tra chạm với objectToHide (thường là vùng mục tiêu)
        if (objectToHide != null)
        {
            RectTransform targetRect = objectToHide.GetComponent<RectTransform>();
            if (targetRect != null && RectTransformUtility.RectangleContainsScreenPoint(targetRect, Input.mousePosition, canvas.worldCamera))
            {
                Debug.Log($"🎯 {gameObject.name} chạm {objectToHide.name}");

                if (objectToHide != null)
                    objectToHide.SetActive(false);

                if (objectToShow != null)
                    objectToShow.SetActive(true);
            }
        }
    }
}
