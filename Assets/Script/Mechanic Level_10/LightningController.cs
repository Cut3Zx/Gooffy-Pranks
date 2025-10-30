using UnityEngine;
using System.Collections;

public class LightningController : MonoBehaviour
{
    [Header("Liên kết đối tượng")]
    public GameObject lightningEffect;   // sprite sét (ẩn sẵn)
    public StoveController stove;        // tham chiếu tới bếp (Ignite)
    public FishController fish;          // để đảm bảo cá chín

    private bool hasTriggered = false;

    // 🔥 Gọi hàm này từ mây trắng khi mây đen vừa được bật
    public void StartLightningSequence()
    {
        if (!hasTriggered)
            StartCoroutine(LightningSequence());
    }

    private IEnumerator LightningSequence()
    {
        SFXManager.Instance.PlaySFX("Thunder");
        SFXManager.Instance.PlaySFX("Fire");
        hasTriggered = true;

        // ⚡ Sét xuất hiện chớp nhanh
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(true);
            Debug.Log("⚡ Sét đánh!");
            yield return new WaitForSeconds(0.5f);
            lightningEffect.SetActive(false);
        }

        // Sau đó đốt bếp
        if (stove != null)
            stove.Ignite();

        yield return null;
    }
}
