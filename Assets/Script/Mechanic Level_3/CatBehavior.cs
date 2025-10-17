using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CatBehavior : BaseObjectManager
{
    [Header("Trạng thái mèo")]
    public GameObject catSleep;
    public GameObject catRun;

    [Header("Mục tiêu")]
    public GameObject balloon;

    [Header("Cài đặt di chuyển")]
    public float moveDuration = 1.5f;

    private bool isAwake = false;

    protected override void Awake()
    {
        base.Awake();
        if (catSleep != null) catSleep.SetActive(true);
        if (catRun != null) catRun.SetActive(false);
    }

    public void WakeUp()
    {
        if (isAwake) return;
        isAwake = true;

        Debug.Log("🐈 Mèo tỉnh dậy — bắt đầu chạy tới bóng bay!");

        if (catSleep != null) catSleep.SetActive(false);
        if (catRun != null) catRun.SetActive(true);

        StartCoroutine(RunToBalloon());
    }

    private IEnumerator RunToBalloon()
    {
        if (catRun == null || balloon == null) yield break;

        Vector3 start = catRun.transform.position;
        Vector3 target = balloon.transform.position;

        for (float t = 0; t < 1; t += Time.deltaTime / moveDuration)
        {
            catRun.transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        Debug.Log("🐾 Mèo chạm bóng bay — bóng nổ!");

        BalloonBurst balloonScript = balloon.GetComponent<BalloonBurst>();
        if (balloonScript != null)
            balloonScript.Burst();

        catRun.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData e) { }
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
