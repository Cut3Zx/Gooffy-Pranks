using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSFX : MonoBehaviour
{
    void Awake()
    {
        // Khi nút này được tạo, tự tìm SoundManager và thêm sự kiện click
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayClick();
        });
    }
}
