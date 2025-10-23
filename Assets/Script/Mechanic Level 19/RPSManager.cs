using UnityEngine;
using System.Collections.Generic;

public class RPSManager : MonoBehaviour
{
    public static RPSManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Xử lý va chạm giữa hai vật (Rock, Paper, Scissors)
    /// </summary>
    public void CheckWin(GameObject obj1, GameObject obj2)
    {
        string tag1 = obj1.tag;
        string tag2 = obj2.tag;

        Debug.Log($"⚔️ {tag1} chạm {tag2}");

        // ✂️ Kéo thắng Bao
        if ((tag1 == "Scissors" && tag2 == "Paper"))
        {
            Debug.Log("✂️ Kéo cắt Bao!");
            obj2.SetActive(false);
        }
        else if ((tag1 == "Paper" && tag2 == "Scissors"))
        {
            Debug.Log("✂️ Kéo cắt Bao!");
            obj1.SetActive(false);
        }

        // ✂️ Kéo thua Búa
        else if ((tag1 == "Scissors" && tag2 == "Rock"))
        {
            Debug.Log("💀 Kéo thua Búa!");
            obj1.SetActive(false);
            LoseImmediately(); // ❌ Kéo biến mất = thua
            return;
        }
        else if ((tag1 == "Rock" && tag2 == "Scissors"))
        {
            Debug.Log("💀 Kéo thua Búa!");
            obj2.SetActive(false);
            LoseImmediately(); // ❌ Kéo biến mất = thua
            return;
        }

        // 📄 Bao thắng Búa
        else if ((tag1 == "Paper" && tag2 == "Rock"))
        {
            Debug.Log("📄 Bao bọc Búa!");
            obj2.SetActive(false);
        }
        else if ((tag1 == "Rock" && tag2 == "Paper"))
        {
            Debug.Log("📄 Bao bọc Búa!");
            obj1.SetActive(false);
        }
        else
        {
            Debug.Log("🤝 Không có gì xảy ra.");
        }

        // ✅ Kiểm tra điều kiện thắng nếu vẫn còn chơi
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

        // Chỉ giữ lại object còn đang active
        remaining.RemoveAll(o => !o.activeInHierarchy);

        int count = remaining.Count;
        Debug.Log($"🧩 Còn lại {count} vật.");

        // ✅ Thắng khi chỉ còn lại Kéo
        if (count == 1 && remaining[0].CompareTag("Scissors"))
        {
            Debug.Log("🏆 Chỉ còn lại Kéo → Thắng!");
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);
        }
    }

    /// <summary>
    /// ❌ Thua ngay lập tức khi Kéo biến mất
    /// </summary>
    private void LoseImmediately()
    {
        Debug.Log("💀 Kéo biến mất → Thua ngay!");
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(false);
    }
}
