using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class BalloonBurst : BaseObjectManager
{
    [Header("Liên kết đến ông hàng xóm sẽ tỉnh dậy")]
    public GameObject neighbor;

    [Header("Hiệu ứng nổ")]
    public GameObject burstSpritePrefab;
    public AudioClip burstSound;
    private AudioSource audioSource;

    [Header("Thời gian delay")]
    public float showBurstDuration = 0.6f;
    public float neighborWakeDelay = 1.2f;

    private bool hasBurst = false;
    private Image balloonImage; // 👈 để ẩn sprite mà không disable object

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        balloonImage = GetComponent<Image>(); // 👈 lấy Image component của bóng
    }

    public void Burst()
    {
        if (hasBurst) return;
        hasBurst = true;

        Debug.Log("🎈 Bóng bay đã nổ!");

        // 🔊 Phát âm thanh nổ (nếu có)
        if (audioSource != null && burstSound != null)
            audioSource.PlayOneShot(burstSound);

        // ⚡Ẩn sprite bóng (chứ không tắt object)
        if (balloonImage != null)
            balloonImage.enabled = false;

        // 🎨 Hiển thị sprite nổ
        StartCoroutine(ShowBurstImage());

        // 🧍‍♂️ Gọi ông hàng xóm dậy
        StartCoroutine(WakeNeighborAfterDelay());
    }

    private IEnumerator ShowBurstImage()
    {
        GameObject burst = null;

        if (burstSpritePrefab != null)
        {
            burst = Instantiate(burstSpritePrefab, transform.parent);
            burst.transform.position = transform.position;
            burst.transform.SetAsLastSibling();
        }

        Debug.Log("💥 Hiện sprite bóng bay nổ!");

        yield return new WaitForSeconds(showBurstDuration);

        if (burst != null)
            Destroy(burst);

        Debug.Log("💨 Ẩn hiệu ứng nổ, tiếp tục chain!");

        // ✅ Sau khi hiệu ứng nổ xong mới ẩn cả object
        gameObject.SetActive(false);
    }

    private IEnumerator WakeNeighborAfterDelay()
    {
        yield return new WaitForSeconds(neighborWakeDelay);

        if (neighbor == null)
        {
            Debug.LogError("❌ neighbor chưa được gán trong BalloonBurst!");
            yield break;
        }

        NeighborSleep neighborScript = neighbor.GetComponent<NeighborSleep>();
        if (neighborScript == null)
            neighborScript = neighbor.GetComponentInChildren<NeighborSleep>();

        if (neighborScript != null)
        {
            Debug.Log("😮 Gọi ông hàng xóm tỉnh dậy (sau hiệu ứng nổ)!");
            neighborScript.WakeUp();
        }
    }

    public override void OnPointerClick(PointerEventData e) { }
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
