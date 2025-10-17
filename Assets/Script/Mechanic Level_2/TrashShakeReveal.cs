using UnityEngine;

public class TrashShakeFall : MonoBehaviour
{
    [Header("Cục giấy rơi ra khi thùng đổ")]
    public GameObject paperCrumpled;

    [Header("Góc đổ (90 độ là đổ hẳn)")]
    public float fallAngle = 90f;

    [Header("Tốc độ xoay thùng rác")]
    public float rotationSpeed = 4f;

    [Header("Độ nhạy lắc điện thoại")]
    public float shakeThreshold = 2.8f;

    [Header("Lực đẩy cục giấy khi rơi ra (theo hướng đổ)")]
    public float pushPower = 3f;
    public float pushUpward = 2f;

    private bool hasFallen = false;
    private Quaternion startRot;
    private Quaternion targetRot;

    void Start()
    {
        startRot = transform.rotation;
        targetRot = Quaternion.Euler(0, 0, -fallAngle); // mặc định đổ sang trái
    }

    void Update()
    {
        // Lắc điện thoại
        if (Input.acceleration.sqrMagnitude > shakeThreshold && !hasFallen)
            StartCoroutine(FallTrash());

        // Click chuột để test trong Unity Editor
        
    }

    private System.Collections.IEnumerator FallTrash()
    {
        hasFallen = true;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Bật cục giấy
        if (paperCrumpled != null)
        {
            paperCrumpled.SetActive(true);
            Rigidbody2D rb = paperCrumpled.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;

                // Xác định hướng đổ của thùng rác
                float dir = Mathf.Sign(transform.right.x); // +1 phải, -1 trái

                // Đẩy cục giấy ra theo hướng đổ
                Vector2 force = new Vector2(dir * pushPower, pushUpward);
                rb.AddForce(force, ForceMode2D.Impulse);

                Debug.Log($"🗑️ Thùng rác đổ (hướng {(dir > 0 ? "phải" : "trái")}), cục giấy bị đẩy ra!");
            }
        }
    }
}
