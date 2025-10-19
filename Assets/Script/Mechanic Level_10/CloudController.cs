using UnityEngine;
using UnityEngine.EventSystems;

public class CloudController : BaseObjectManager
{
    [Header("Liên kết đối tượng")]
    public GameObject darkCloud;     // mây đen (ẩn sẵn)
    public LightningController lightning; // script trong mây đen

    private bool hasTriggered = false;

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (!hasTriggered)
        {
            hasTriggered = true;
            ShowDarkCloud();
        }

        ResetPosition();
    }

    private void ShowDarkCloud()
    {
        // 1️⃣ Ẩn mây trắng
        gameObject.SetActive(false);

        // 2️⃣ Hiện mây đen
        if (darkCloud != null)
        {
            darkCloud.SetActive(true);
            Debug.Log("🌑 Mây đen xuất hiện");

            // 3️⃣ Gọi script trong mây đen để đánh sét
            if (lightning != null)
                lightning.StartLightningSequence();
        }
    }
}
