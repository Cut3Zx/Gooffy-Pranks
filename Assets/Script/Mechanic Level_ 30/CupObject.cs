using UnityEngine;
using UnityEngine.UI;

public class CupObject : MonoBehaviour
{
    [Header("🥤 Sprite các trạng thái")]
    public Sprite fullCupSprite;
    public Sprite emptyCupSprite;

    [Header("Loại cốc")]
    public bool isFakeCup = false; // cốc giả = không mất nước

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
            Debug.LogWarning($"{name} chưa có Image component!");
    }

    public void OnShake()
    {
        if (isFakeCup)
        {
            Debug.Log($"🟢 {name} là cốc giả → vẫn còn nước!");
            return;
        }

        if (emptyCupSprite != null && image != null)
        {
            image.sprite = emptyCupSprite;
            Debug.Log($"💧 {name} bị đổ nước!");
        }
    }
}
