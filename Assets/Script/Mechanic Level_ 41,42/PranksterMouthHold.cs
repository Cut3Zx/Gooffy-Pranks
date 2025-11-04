using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PranksterMouthHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Prankster các trạng thái")]
    public GameObject pranksterNormal;     // Prankster bình thường
    public GameObject pranksterMouthHold;  // Prankster ngậm mồm / ngạt thở
    public GameObject pranksterFat;        // Prankster phồng to (béo, để bay)

    [Header("Thời gian giữ để phồng lên (giây)")]
    public float holdTime = 1.5f;

    private bool isHolding = false;
    private Coroutine holdCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isHolding) return;

        isHolding = true;
        holdCoroutine = StartCoroutine(HoldMouthSequence());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Nếu nhả tay sớm thì hủy phồng
        if (isHolding)
        {
            isHolding = false;
            if (holdCoroutine != null)
                StopCoroutine(holdCoroutine);

            // Quay lại trạng thái bình thường
            pranksterMouthHold.SetActive(false);
            pranksterNormal.SetActive(true);
        }
    }

    private IEnumerator HoldMouthSequence()
    {
        // Bắt đầu bị ngậm
        pranksterNormal.SetActive(false);
        pranksterMouthHold.SetActive(true);

        float timer = 0f;
        while (timer < holdTime && isHolding)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (isHolding)
        {
            // Giữ đủ lâu → phồng lên
            pranksterMouthHold.SetActive(false);
            pranksterFat.SetActive(true);
            SFXManager.Instance.PlaySFX("Balloon");
            Debug.Log("🎈 Prankster phồng lên — sẵn sàng bay!");
        }

        isHolding = false;
    }
}
