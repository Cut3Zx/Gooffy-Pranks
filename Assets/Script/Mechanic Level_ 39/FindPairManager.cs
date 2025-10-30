using UnityEngine;
using System.Collections;

public class FindPairManager : MonoBehaviour
{
    public static FindPairManager Instance;

    [Header("UI thắng khi hoàn thành")]
    public GameObject winUI;

    [Header("Tổng số cặp thẻ trong màn")]
    public int totalPairs = 5;

    private CardController firstCard;
    private CardController secondCard;
    private int matchedPairs = 0;
    private bool isChecking = false;
    public bool IsChecking() => isChecking;
    void Awake()
    {
        Instance = this;
    }

    public void CheckCard(CardController card)
    {
        // 🚫 Chặn khi đang kiểm tra hoặc khi card null / bị disable
        if (isChecking || card == null || !card.gameObject.activeInHierarchy) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isChecking = true;
        yield return new WaitForSeconds(0.35f); // delay nhỏ để người chơi thấy

        if (firstCard == null || secondCard == null)
        {
            ResetCards();
            yield break;
        }

        if (firstCard.cardID == secondCard.cardID)
        {
            Debug.Log($"✅ Khớp: {firstCard.cardID}");
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchedPairs++;

            yield return new WaitForSeconds(0.4f); // đợi animation biến mất

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("🏆 WIN — đủ cặp rồi!");
                if (winUI) winUI.SetActive(true);

                if (GameManager.Instance != null)
                    GameManager.Instance.EndGame(true);

                
            }
        }
        else
        {
            Debug.Log($"❌ Sai cặp: {firstCard.cardID} vs {secondCard.cardID}");
            firstCard.Flip(false);
            secondCard.Flip(false);
            yield return new WaitForSeconds(0.3f);
        }

        ResetCards();
    }

    private void ResetCards()
    {
        firstCard = null;
        secondCard = null;
        isChecking = false;
    }
}
