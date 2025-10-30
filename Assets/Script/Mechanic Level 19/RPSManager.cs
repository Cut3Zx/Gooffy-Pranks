using UnityEngine;
using System.Collections.Generic;

public class RPSManager : MonoBehaviour
{
    public static RPSManager Instance;

    private GameManager gameManager;

    private void Awake()
    {
        Instance = this;

        // 🔗 Tự động tham chiếu GameManager nếu có
        if (GameManager.Instance != null)
            gameManager = GameManager.Instance;
        else
            gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Xử lý va chạm giữa hai vật (Rock, Paper, Scissors)
    /// </summary>
    public void CheckWin(GameObject obj1, GameObject obj2)
    {
        string tag1 = obj1.tag;
        string tag2 = obj2.tag;

        Debug.Log($"⚔️ {tag1} chạm {tag2}");

        // ✂️ Kéo cắt Bao
        if (tag1 == "Scissors" && tag2 == "Paper")
        {
            Debug.Log("✂️ Kéo cắt Bao → Bao biến mất!");
            SFXManager.Instance.PlaySFX("Keo");
            obj2.SetActive(false);
        }
        else if (tag1 == "Paper" && tag2 == "Scissors")
        {
            Debug.Log("✂️ Bao chạm Kéo → Bao bị cắt mất!");
            SFXManager.Instance.PlaySFX("Keo");
            obj1.SetActive(false);
        }

        // 📄 Bao bọc Búa
        else if (tag1 == "Paper" && tag2 == "Rock")
        {
            Debug.Log("📄 Bao bọc Búa → Búa biến mất!");
            SFXManager.Instance.PlaySFX("Bao");
            obj2.SetActive(false);
        }
        else if (tag1 == "Rock" && tag2 == "Paper")
        {
            Debug.Log("📄 Búa chạm Bao → Búa bị bọc mất!");
            SFXManager.Instance.PlaySFX("Bao");
            obj1.SetActive(false);
        }

        // 🪨 Búa đập Kéo
        else if (tag1 == "Rock" && tag2 == "Scissors")
        {
            Debug.Log("🪨 Búa đập Kéo → Kéo biến mất!");
            SFXManager.Instance.PlaySFX("Bua");
            obj2.SetActive(false);
            LoseImmediately();
            return;
        }
        else if (tag1 == "Scissors" && tag2 == "Rock")
        {
            Debug.Log("🪨 Kéo chạm Búa → Kéo bị đập vỡ!");
            SFXManager.Instance.PlaySFX("Bua");
            obj1.SetActive(false);
            LoseImmediately();
            return;
        }

        else
        {
            Debug.Log("🤝 Không có gì xảy ra.");
        }

        // Kiểm tra xem còn lại bao nhiêu vật thể
        CheckRemainingObjects();
    }

    /// <summary>
    /// Kiểm tra còn lại bao nhiêu vật sau va chạm để xác định thắng/thua
    /// </summary>
    private void CheckRemainingObjects()
    {
        List<GameObject> remaining = new List<GameObject>();
        remaining.AddRange(GameObject.FindGameObjectsWithTag("Rock"));
        remaining.AddRange(GameObject.FindGameObjectsWithTag("Paper"));
        remaining.AddRange(GameObject.FindGameObjectsWithTag("Scissors"));

        // Lọc ra object còn đang active
        remaining.RemoveAll(o => !o.activeInHierarchy);

        int count = remaining.Count;
        Debug.Log($"🧩 Còn lại {count} vật.");

        // ✅ Thắng khi chỉ còn lại Kéo
        if (count == 1 && remaining[0].CompareTag("Scissors"))
        {
            Debug.Log("🏆 Chỉ còn lại Kéo → Thắng!");

            // 🖼 Gọi hiệu ứng thắng + chụp ảnh
            if (gameManager != null)
            {
                // Gọi đúng chuỗi xử lý thắng trong GameManager
                gameManager.EndGame(true);
            }
        }
    }

    /// <summary>
    /// ❌ Thua ngay lập tức khi Kéo biến mất
    /// </summary>
    private void LoseImmediately()
    {
        Debug.Log("💀 Kéo biến mất → Thua ngay!");
        if (gameManager != null)
            gameManager.EndGame(false);
    }
}
