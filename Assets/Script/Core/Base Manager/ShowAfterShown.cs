using UnityEngine;
using System.Collections;

public class ShowAfterShown : MonoBehaviour
{
    [Header("🎯 Object sẽ hiển thị sau khi object này xuất hiện")]
    public GameObject nextObject;

    [Header("⏱️ Thời gian chờ trước khi hiển thị object tiếp theo")]
    public float delay = 0.5f;

    private bool triggered = false;

    void OnEnable()
    {
        if (!triggered)
            StartCoroutine(ShowNextAfterDelay());
    }

    private IEnumerator ShowNextAfterDelay()
    {
        triggered = true; // tránh lặp
        yield return new WaitForSeconds(delay);

        if (nextObject != null)
        {
            nextObject.SetActive(true);
            Debug.Log($"✨ {nextObject.name} được hiển thị sau {delay} giây!");
        }
    }
}
