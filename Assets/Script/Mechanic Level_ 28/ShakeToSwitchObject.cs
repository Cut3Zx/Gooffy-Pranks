using UnityEngine;

public class ShakeToSwitchObject : MonoBehaviour
{
    [Header("📱 Độ nhạy lắc (càng nhỏ càng nhạy)")]
    public float shakeThreshold = 2.5f;

    [Header("🟢 Object sẽ ẩn khi lắc")]
    public GameObject objectToHide;

    [Header("🔵 Object sẽ hiện khi lắc")]
    public GameObject objectToShow;

    private Vector3 lastAccel;
    private bool switched = false;

    void Start()
    {
        lastAccel = Input.acceleration;
    }

    void Update()
    {
        // 🖱️ Test bằng phím cách trong Editor
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleObjects();
        }

        // 📱 Phát hiện lắc mạnh
        Vector3 delta = Input.acceleration - lastAccel;
        if (delta.sqrMagnitude > shakeThreshold * shakeThreshold && !switched)
        {
            ToggleObjects();
        }

        lastAccel = Input.acceleration;
    }

    private void ToggleObjects()
    {
        switched = true;

        if (objectToHide != null)
            objectToHide.SetActive(false);

        if (objectToShow != null)
            objectToShow.SetActive(true);

        Debug.Log("📱 Lắc mạnh! Đã ẩn object cũ và hiện object mới!");
    }
}
