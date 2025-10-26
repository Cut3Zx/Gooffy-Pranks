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
    public float pushPower = 5f;
    public float pushUpward = 3f;

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
        // 📱 Lắc điện thoại
        if (Input.acceleration.sqrMagnitude > shakeThreshold && !hasFallen)
            StartCoroutine(FallTrash());

        // 🖱️ Test bằng chuột hoặc phím cách
        if (( Input.GetKeyDown(KeyCode.Space)) && !hasFallen)
            StartCoroutine(FallTrash());
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

        yield return new WaitForSeconds(0.15f);

        // 🗑️ Bật cục giấy
        if (paperCrumpled != null)
        {
            paperCrumpled.SetActive(true);
            Rigidbody2D rb = paperCrumpled.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.gravityScale = 2f; // ✅ rơi nhanh và thật hơn

                // Xác định hướng đổ
                float dir = (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) ? -1f : 1f;
                // +1 phải, -1 trái

                // Thêm lực mạnh và thật hơn
                rb.AddForce(new Vector2(dir * pushPower, pushUpward), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-3f, 3f), ForceMode2D.Impulse); // thêm xoay nhẹ cho tự nhiên

                Debug.Log($"🗑️ Thùng rác đổ ({(dir > 0 ? "phải" : "trái")}), cục giấy rơi ra thật!");
            }
        }
    }
}
