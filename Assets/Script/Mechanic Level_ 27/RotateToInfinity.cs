using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EightToInfinityOnPull : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Cấu hình kéo")]
    public float dragThreshold = 80f;   // Kéo sang trái/phải bao nhiêu pixel thì kích hoạt xoay
    public float rotationSpeed = 300f;  // Tốc độ xoay (độ/giây)
    public float targetAngle = 90f;     // Xoay ngang 90 độ

    [Header("UI Thắng")]
    public GameObject winUI;

    private Vector2 startDragPos;
    private bool isRotating = false;
    private bool hasWon = false;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hasWon || isRotating) return;
        startDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // không làm gì khi đang kéo – chỉ theo dõi vị trí
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hasWon || isRotating) return;

        float dragDistanceX = eventData.position.x - startDragPos.x;
        float dragDistanceY = eventData.position.y - startDragPos.y;

        Debug.Log($"📏 Kéo ngang: {dragDistanceX}, dọc: {dragDistanceY}");

        // chỉ tính kéo ngang đủ xa
        if (Mathf.Abs(dragDistanceX) >= dragThreshold)
        {
            StartCoroutine(RotateAndWin());
        }
    }

    private IEnumerator RotateAndWin()
    {
        isRotating = true;

        float startZ = rectTransform.eulerAngles.z;
        float endZ = startZ + targetAngle;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * (rotationSpeed / targetAngle);
            float z = Mathf.Lerp(startZ, endZ, t);
            rectTransform.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        isRotating = false;
        hasWon = true;

        Debug.Log("♾️ Số 8 đã xoay thành vô cực!");

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);

        if (winUI != null)
            winUI.SetActive(true);
    }
}
