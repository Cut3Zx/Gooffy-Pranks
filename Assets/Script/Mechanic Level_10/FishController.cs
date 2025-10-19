using UnityEngine;
using UnityEngine.EventSystems;

public class FishController : BaseObjectManager
{
    public Sprite rawFish;
    public Sprite cookedFish;

    private bool isCooked = false;
    private UnityEngine.UI.Image img;

    protected override void Awake()
    {
        base.Awake();
        img = GetComponent<UnityEngine.UI.Image>();
        img.sprite = rawFish;
    }

    public void CookFish()
    {
        isCooked = true;
        img.sprite = cookedFish;
        Debug.Log("🐟 Cá đã chín!");
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (isCooked)
        {
            Debug.Log("🏆 Ăn cá → thắng!");
            GameManager.Instance.EndGame(true);
        }
    }

    public override void OnDrag(PointerEventData eventData) { } // cá không kéo
}
