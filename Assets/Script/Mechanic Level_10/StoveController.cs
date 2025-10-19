using UnityEngine;
using System.Collections;

public class StoveController : BaseObjectManager
{
    public GameObject fireEffect;  // sprite lửa (ẩn sẵn)
    public FishController fish;
    private bool isLit = false;

    public void Ignite()
    {
        if (isLit) return;
        StartCoroutine(IgniteSequence());
    }

    private IEnumerator IgniteSequence()
    {
        isLit = true;
        if (fireEffect != null) fireEffect.SetActive(true);
        Debug.Log("🔥 Bếp cháy!");

        // 2 giây sau cá chín
        yield return new WaitForSeconds(2f);

        if (fish != null)
            fish.CookFish();

        // 1 giây sau thắng
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
    }
}
