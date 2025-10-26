using UnityEngine;
using System.Collections;

public class FindPairManager : MonoBehaviour
{
    public static FindPairManager Instance;

    [Header("UI thắng khi hoàn thành")]
    public GameObject winUI;

    private CardController firstCard;
    private CardController secondCard;
    private int matchedPairs = 0;

    [Header("Tổng số cặp thẻ trong màn")]
    public int totalPairs = 5;

    void Awake()
    {
        Instance = this;
    }

    public void CheckCard(CardController card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f); // delay nhỏ để người chơi nhìn rõ

        if (firstCard.cardID == secondCard.cardID)
        {
            Debug.Log($"✅ Khớp: {firstCard.cardID}");
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchedPairs++;

            // Nếu đã tìm đủ cặp → thắng
            if (matchedPairs >= totalPairs)
            {
                Debug.Log("🏆 Thắng rồi!");
                yield return new WaitForSeconds(0.5f);
                if (winUI) winUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log($"❌ Sai cặp: {firstCard.cardID} vs {secondCard.cardID}");
            firstCard.Flip(false);
            secondCard.Flip(false);
        }

        // reset
        firstCard = null;
        secondCard = null;
    }
}
