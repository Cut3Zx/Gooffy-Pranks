using UnityEngine;
using UnityEngine.EventSystems;

public class BurgerFeedPrankster : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prankster các trạng thái")]
    public GameObject pranksterNormal;
    public GameObject pranksterMouthOpen;
    public GameObject pranksterFull;

    [Header("Khoảng cách tương tác")]
    public float openMouthDistance = 200f;
    public float eatDistance = 120f;

    private RectTransform rect;
    private Canvas canvas;
    private RectTransform pranksterRect;
    private bool isDragging = false;
    private bool hasEaten = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (pranksterNormal != null)
            pranksterRect = pranksterNormal.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (hasEaten) return;
        isDragging = true;
    }

    public void OnDrag(PointerEventData e)
    {
        if (!isDragging || hasEaten) return;
        rect.anchoredPosition += e.delta / canvas.scaleFactor;

        // Nếu gần thì há miệng
        if (pranksterRect != null)
        {
            float dist = Vector2.Distance(rect.anchoredPosition, pranksterRect.anchoredPosition);
            bool shouldOpen = dist < openMouthDistance;

            pranksterNormal.SetActive(!shouldOpen);
            pranksterMouthOpen.SetActive(shouldOpen);
        }
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (hasEaten) return;

        float dist = Vector2.Distance(rect.anchoredPosition, pranksterRect.anchoredPosition);

        if (dist < eatDistance)
            EatBurger();
    }

    private void EatBurger()
    {
        hasEaten = true;
        pranksterNormal.SetActive(false);
        pranksterMouthOpen.SetActive(false);
        pranksterFull.SetActive(true);

        // Ẩn burger
        gameObject.SetActive(false);

        Debug.Log("😋 Prankster đã ăn burger to!");
    }
}
