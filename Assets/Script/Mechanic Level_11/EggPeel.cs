using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EggPeel : BaseObjectManager
{
    [Header("Thành phần trứng")]
    public GameObject eggWhole;      // quả trứng nguyên
    public GameObject crackedEgg;    // object chứa crackTop + crackBottom (inactive ban đầu)
    public AudioSource sfx;          // tiếng bóc vỏ (nếu có)

    [Header("Cài đặt double tap")]
    public float doubleTapWindow = 0.35f;

    private float lastTapTime = -10f;
    private int tapCount = 0;
    private bool peeled = false;

    protected override void Awake()
    {
        base.Awake();
        if (eggWhole != null) eggWhole.SetActive(true);
        if (crackedEgg != null) crackedEgg.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (peeled) return;

        float now = Time.time;
        if (now - lastTapTime <= doubleTapWindow)
        {
            tapCount++;
        }
        else
        {
            tapCount = 1;
        }
        lastTapTime = now;

        if (tapCount == 2)
        {
            PeelEgg();
        }
    }

    private void PeelEgg()
    {
        peeled = true;

        if (eggWhole != null) eggWhole.SetActive(false);
        if (crackedEgg != null) crackedEgg.SetActive(true);

        if (sfx != null) sfx.Play();

        Debug.Log("🥚 Trứng nứt ra!");
    }

    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
