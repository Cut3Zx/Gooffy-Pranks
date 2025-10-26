using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EightToInfinityOnPull : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("⚙️ Cấu hình kéo")]
    public float dragThreshold = 80f;   // Kéo ngang bao nhiêu pixel thì xoay
    public float rotationSpeed = 300f;  // Tốc độ xoay
    public float targetAngle = 90f;     // Xoay ngang 90 độ

    [Header("🎭 Prankster đổi mặt")]
    public GameObject pranksterSad;     // Prankster buồn
    public GameObject pranksterHappy;   // Prankster vui (ẩn sẵn)

    private Vector2 startDragPos;
    private bool isRotating = false;
    private bool hasRotated = false;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (pranksterHappy != null)
            pranksterHappy.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (hasRotated || isRotating) return;
        startDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (hasRotated || isRotating) return;

        float dragDistanceX = eventData.position.x - startDragPos.x;
        if (Mathf.Abs(dragDistanceX) >= dragThreshold)
        {
            StartCoroutine(RotateThenChangeFace());
        }
    }

    private IEnumerator RotateThenChangeFace()
    {
        isRotating = true;

        float startZ = rectTransform.eulerAngles.z;
        float endZ = startZ + targetAngle;
        float t = 0;

        // 🌀 Xoay số 8
        while (t < 1f)
        {
            t += Time.deltaTime * (rotationSpeed / targetAngle);
            float z = Mathf.Lerp(startZ, endZ, t);
            rectTransform.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        // 😢 → 😄
        if (pranksterSad != null)
            pranksterSad.SetActive(false);

        if (pranksterHappy != null)
            pranksterHappy.SetActive(true);

        Debug.Log("♾️ Số 8 đã xoay xong — Prankster vui lên!");

        hasRotated = true;
        isRotating = false;
    }
}
