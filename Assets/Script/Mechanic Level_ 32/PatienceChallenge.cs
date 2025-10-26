using UnityEngine;
using System.Collections;

public class PatienceChallenge : MonoBehaviour
{
    [Header("⏱ Thời gian cần chờ để thắng")]
    public float waitTime = 8f;

    [Header("🎉 UI Thắng / Thua")]
    public GameObject winUI;
    public GameObject loseUI;

    private bool hasEnded = false;

    void Start()
    {
        StartCoroutine(WaitForWin());
    }

    void Update()
    {
        // Nếu người chơi chạm vào bất cứ đâu → Thua ngay
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.anyKeyDown && !hasEnded)
        {
            LoseGame();
        }
#endif

        if (Input.touchCount > 0 && !hasEnded)
        {
            LoseGame();
        }
    }

    IEnumerator WaitForWin()
    {
        yield return new WaitForSeconds(waitTime);

        if (!hasEnded)
        {
            hasEnded = true;
            Debug.Log("⏳ Người chơi đã kiên nhẫn chờ đủ thời gian → Thắng!");
            if (winUI != null) winUI.SetActive(true);
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);
        }
    }

    private void LoseGame()
    {
        hasEnded = true;
        Debug.Log("❌ Người chơi không kiên nhẫn → Thua!");

        if (loseUI != null)
            loseUI.SetActive(true);

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(false);
    }
}
