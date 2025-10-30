using UnityEngine;
using UnityEngine.EventSystems;

public class HideAndShowUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Âm thanh khi chạm")]
    public string touchSoundName;

    [Header("Các đối tượng cần ẩn khi chạm")]
    public GameObject objectToHide1;
    public GameObject objectToHide2;
    public GameObject objectToHide3;

    [Header("Các đối tượng cần hiện ra khi chạm")]
    public GameObject objectToShow1;
    public GameObject objectToShow2;

    private RectTransform rect;
    private Canvas canvas;
    private Vector2 startPos;
    private int originalIndex;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        originalIndex = transform.GetSiblingIndex(); // chỉ lưu thứ tự gốc
                                                     // không đưa lên top
    }


    public void OnDrag(PointerEventData e)
    {
        rect.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        // Nếu chạm vùng ẩn chính
        if (objectToHide1 != null)
        {
            RectTransform targetRect = objectToHide1.GetComponent<RectTransform>();
            if (targetRect && RectTransformUtility.RectangleContainsScreenPoint(targetRect, Input.mousePosition, canvas.worldCamera))
            {
                if (SFXManager.Instance != null && !string.IsNullOrEmpty(touchSoundName))
                {
                    SFXManager.Instance.PlaySFX(touchSoundName);
                }

                Debug.Log($"🎯 {name} chạm {objectToHide1.name}");

                // Ẩn
                if (objectToHide1) objectToHide1.SetActive(false);
                if (objectToHide2) objectToHide2.SetActive(false);
                if (objectToHide3) objectToHide3.SetActive(false);

                // Hiện
                if (objectToShow1) objectToShow1.SetActive(true);
                if (objectToShow2) objectToShow2.SetActive(true);
            }
        }

        // Quay lại vị trí cũ
        rect.anchoredPosition = startPos;
        transform.SetSiblingIndex(originalIndex);
    }
}
