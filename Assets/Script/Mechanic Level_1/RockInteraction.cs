using UnityEngine;

public class RockInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject brokenRock;  // Ảnh đá vỡ
    public GameObject chicken;     // Ảnh con gà

    private bool isBroken = false;

    public void BreakRock()
    {
        if (isBroken) return;
        isBroken = true;

        Debug.Log("💥 Đập đá - hiện đá vỡ và gà cùng lúc!");

        // Ẩn cục đá nguyên vẹn (chỉ tắt Image chứ không tắt toàn bộ object)
        var image = GetComponent<UnityEngine.UI.Image>();
        if (image != null)
            image.enabled = false;

        // Hiện đá vỡ
        if (brokenRock != null)
            brokenRock.SetActive(true);

        // Hiện gà
        if (chicken != null)
            chicken.SetActive(true);
    }
}
