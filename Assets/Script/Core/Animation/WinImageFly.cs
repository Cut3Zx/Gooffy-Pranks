//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;

//public class WinImageEffect : MonoBehaviour
//{
//    [Header("References")]
//    public RectTransform targetTransform;   // 🎯 túi đồ (đích đến)
//    public GameObject bagImage;             // 👜 hình túi đồ hiển thị cùng ảnh thắng
//    public Image darkBackground;            // 🌑 nền đen mờ phía sau

//    [Header("Animation Settings")]
//    public float delayBeforeSpin = 1f;
//    public float duration = 1.2f;
//    public float totalRotation = 720f;
//    public float finalScaleFactor = 0.3f;
//    public float backgroundFadeSpeed = 0.5f;

//    private CanvasGroup group;
//    private RectTransform rect;

//    private void Awake()
//    {
//        rect = GetComponent<RectTransform>();
//        group = GetComponent<CanvasGroup>();
//        if (group == null)
//            group = gameObject.AddComponent<CanvasGroup>();
//    }

//    public void PlayEffect()
//    {
//        if (targetTransform == null)
//        {
//            Debug.LogWarning("⚠️ WinImageEffect: Chưa gán targetTransform (túi đồ)!");
//            return;
//        }

//        StartCoroutine(AnimateWinImage());
//    }

//    private IEnumerator AnimateWinImage()
//    {
//        // 🎬 Bước 1: Hiện nền đen và túi đồ
//        if (darkBackground != null)
//        {
//            darkBackground.gameObject.SetActive(true);
//            StartCoroutine(FadeBackground(0f, 0.6f)); // nền đen mờ 60%
//        }

//        if (bagImage != null)
//            bagImage.SetActive(true);

//        // Reset trạng thái ban đầu của ảnh thắng
//        Vector2 startPos = rect.anchoredPosition;
//        Vector3 startScale = rect.localScale;
//        rect.localEulerAngles = Vector3.zero;
//        group.alpha = 1f;

//        yield return new WaitForSecondsRealtime(delayBeforeSpin);

//        // 🎡 Bước 2: Xoay + bay + thu nhỏ
//        float elapsed = 0f;
//        Vector2 targetPos = targetTransform.anchoredPosition;

//        while (elapsed < duration)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            float t = elapsed / duration;

//            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
//            rect.localScale = Vector3.Lerp(startScale, startScale * finalScaleFactor, t);
//            rect.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, totalRotation, t));
//            group.alpha = Mathf.Lerp(1f, 0f, t);

//            yield return null;
//        }

//        // 🧹 Bước 3: Ẩn toàn bộ
//        if (bagImage != null)
//            bagImage.SetActive(false);

//        if (darkBackground != null)
//            StartCoroutine(FadeBackground(0.6f, 0f, true));

//        gameObject.SetActive(false);
//    }

//    private IEnumerator FadeBackground(float from, float to, bool disableAfter = false)
//    {
//        Color c = darkBackground.color;
//        float elapsed = 0f;
//        while (elapsed < backgroundFadeSpeed)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            float t = elapsed / backgroundFadeSpeed;
//            c.a = Mathf.Lerp(from, to, t);
//            darkBackground.color = c;
//            yield return null;
//        }

//        if (disableAfter)
//            darkBackground.gameObject.SetActive(false);
//    }
//}
