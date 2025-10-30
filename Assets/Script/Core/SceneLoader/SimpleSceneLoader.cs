using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SimpleSceneLoader : MonoBehaviour
{
    public static SimpleSceneLoader I;

    [Header("Assign các object CON của prefab")]
    public GameObject panel;     // UI panel loading (chính)
    public Slider progressBar;   // Slider tiến trình

    [Header("Tuỳ chọn")]
    public float minShowTime = 0.4f;

    [Header("Ẩn toàn bộ UI khác khi loading")]
    public bool hideAllUI = true;

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);          // sống qua các scene
        if (panel) panel.SetActive(false);
        if (progressBar) progressBar.value = 0f;
    }

    // ✅ Hàm Load chính
    public void Load(string sceneName)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // 🔹 TẮT toàn bộ UI hiện có (nếu cần)
        if (hideAllUI)
            HideAllUI();

        StartCoroutine(LoadRoutine(sceneName));
    }

    IEnumerator LoadRoutine(string scene)
    {
        if (panel) panel.SetActive(true);
        if (progressBar) progressBar.value = 0f;

        var op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;

        float shown = 0f;
        while (op.progress < 0.9f)
        {
            if (progressBar)
                progressBar.value = Mathf.Clamp01(op.progress / 0.9f);
            shown += Time.unscaledDeltaTime;
            yield return null;
        }

        if (progressBar) progressBar.value = 1f;

        while (shown < minShowTime)
        {
            shown += Time.unscaledDeltaTime;
            yield return null;
        }

        op.allowSceneActivation = true;
        yield return null;

        // ✅ Tắt loading panel sau khi load xong
        if (panel) panel.SetActive(false);
    }

    // 🧹 Hàm ẩn toàn bộ UI hiện có khi loading
    private void HideAllUI()
    {
        // Ẩn UI của GameManager
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.winUI != null)
                GameManager.Instance.winUI.SetActive(false);
            if (GameManager.Instance.loseUI != null)
                GameManager.Instance.loseUI.SetActive(false);
            if (GameManager.Instance.timerText != null)
                GameManager.Instance.timerText.gameObject.SetActive(false);
            if (GameManager.Instance.winImage != null)
                GameManager.Instance.winImage.SetActive(false);
        }

        // Ẩn Banner hoặc UI tạm
        var banner = GameObject.Find("BANNER(Clone)");
        if (banner) banner.SetActive(false);

        // Ẩn tất cả các canvas con trừ loading panel
        Canvas[] allCanvas = FindObjectsOfType<Canvas>();
        foreach (Canvas c in allCanvas)
        {
            if (panel != null && c.gameObject == panel) continue;
            c.enabled = false;
        }

        Debug.Log("🧱 Toàn bộ UI đã bị ẩn trong khi loading scene...");
    }
}
