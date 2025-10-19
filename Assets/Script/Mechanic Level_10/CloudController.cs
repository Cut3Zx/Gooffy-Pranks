using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CloudController : BaseObjectManager
{
    public GameObject sun;
    public GameObject lightning;
    public StoveController stove;

    private bool triggered = false;

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // chỉ kích hoạt một lần
        if (triggered) return;

        // nếu kéo mây gần mặt trời
        if (Vector2.Distance(transform.position, sun.transform.position) < 100f)
        {
            triggered = true;
            Debug.Log("☁️ Mây đã che mặt trời → tạo sét!");
            StartCoroutine(ShowLightningAndIgnite());
        }

        // mây quay lại vị trí cũ
        ResetPosition();
    }

    private IEnumerator ShowLightningAndIgnite()
    {
        // Ẩn mặt trời, hiện sét
        sun.SetActive(false);
        lightning.SetActive(true);

        yield return new WaitForSeconds(0.5f); // ⚡ hiển thị sét trong 0.5s

        lightning.SetActive(false);
        Debug.Log("⚡ Sét đánh trúng bếp!");

        if (stove != null)
            stove.Ignite(); // 🔥 kích hoạt bếp cháy
    }
}
