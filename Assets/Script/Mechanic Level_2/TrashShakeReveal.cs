using UnityEngine;

public class TrashShakeReveal : MonoBehaviour
{
    [Header("Cục giấy sẽ rơi ra khi thùng đổ")]
    public GameObject paperCrumpled;

    [Header("Tốc độ xoay khi đổ")]
    public float tiltAngle = 25f;
    public float tiltSpeed = 3f;

    private bool hasFallen = false;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Kiểm tra lắc điện thoại (mobile)
        if (Input.acceleration.sqrMagnitude > 2.5f && !hasFallen)
        {
            StartCoroutine(TiltAndReveal());
        }

        // Hoặc mô phỏng bằng phím "R" trên PC
        if (Input.GetKeyDown(KeyCode.R) && !hasFallen)
        {
            StartCoroutine(TiltAndReveal());
        }
    }

    private System.Collections.IEnumerator TiltAndReveal()
    {
        hasFallen = true;

        // Xoay nghiêng thùng rác
        float t = 0;
        Quaternion targetRot = Quaternion.Euler(0, 0, tiltAngle);
        while (t < 1)
        {
            t += Time.deltaTime * tiltSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRot, t);
            yield return null;
        }

        // Hiện cục giấy sau khi thùng đổ
        if (paperCrumpled != null)
            paperCrumpled.SetActive(true);

        Debug.Log("🗑️ Thùng rác đổ, cục giấy rơi ra!");
    }
}
