using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WrongClickAnywhere : MonoBehaviour
{
    [Header("❌ Prefab dấu X (UI)")]
    public GameObject wrongMarkPrefab;

    [Header("Canvas để spawn UI")]
    public Canvas targetCanvas;

    [Header("Thời gian hiển thị dấu X (giây)")]
    public float showTime = 0.8f;

    [Header("Tên hoặc tag các UI chặn click (ví dụ: Setting, Pause, Win...)")]
    public List<string> blockingUINames = new List<string> { "SettingUI", "PauseUI", "Congrat", "GameOver" };
    public List<string> blockingTags = new List<string> { "BlockUI" }; // nếu bạn đặt tag cho UI

    private Camera uiCamera;
    private RectTransform canvasRect;

    void Awake()
    {
        if (targetCanvas == null) targetCanvas = FindAnyObjectByType<Canvas>();
        uiCamera = targetCanvas ? targetCanvas.worldCamera : Camera.main;
        if (targetCanvas != null) canvasRect = targetCanvas.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 🚫 Nếu có UI đang bật => KHÔNG spawn ❌
            if (IsAnyBlockingUIActive()) return;

            Vector2 screenPos = Input.mousePosition;

            // 🧱 Bỏ qua nếu click ngoài vùng canvas game
            if (canvasRect != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out var local);
                if (!canvasRect.rect.Contains(local))
                    return;
            }

            // 🔍 Kiểm tra UI đúng
            if (EventSystem.current != null)
            {
                var ped = new PointerEventData(EventSystem.current) { position = screenPos };
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(ped, results);

                foreach (var r in results)
                {
                    if (r.gameObject.GetComponent<AllowCorrectClick>() != null)
                        return;
                    if (r.gameObject.GetComponent<Button>() != null)
                        return;
                }
            }

            // 🔎 Kiểm tra vật thể 2D/3D đúng
            if (Camera.main != null)
            {
                Vector3 world = Camera.main.ScreenToWorldPoint(screenPos);
                var hit2D = Physics2D.OverlapPoint((Vector2)world);
                if (hit2D && hit2D.GetComponent<AllowCorrectClick>() != null) return;

                var ray = Camera.main.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out var hit3D))
                    if (hit3D.collider.GetComponent<AllowCorrectClick>() != null) return;
            }

            // 🟥 Không đúng => hiện ❌
            SpawnWrongMark(screenPos);
        }
    }

    void SpawnWrongMark(Vector2 screenPos)
    {
        if (wrongMarkPrefab == null || targetCanvas == null) return;

        GameObject mark = Instantiate(wrongMarkPrefab, targetCanvas.transform);
        mark.transform.SetAsLastSibling();

        var rt = mark.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetCanvas.transform as RectTransform, screenPos, uiCamera, out var local);
        rt.anchoredPosition = local;
        mark.SetActive(true);
        Destroy(mark, showTime);
    }

    // 🧩 Kiểm tra có UI nào đang bật (tự tìm theo name hoặc tag)
    bool IsAnyBlockingUIActive()
    {
        // Tìm theo tên
        foreach (string uiName in blockingUINames)
        {
            GameObject obj = GameObject.Find(uiName);
            if (obj != null && obj.activeInHierarchy)
                return true;
        }

        // Tìm theo tag
        foreach (string tag in blockingTags)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject o in objs)
                if (o.activeInHierarchy)
                    return true;
        }

        return false;
    }
}
