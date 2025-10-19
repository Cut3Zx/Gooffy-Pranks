using UnityEngine;
using UnityEngine.UI;

public class FishController : BaseObjectManager
{
    public Sprite rawFish;
    public Sprite cookedFish;

    private Image image;
    private bool isCooked = false;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }

    public void CookFish()
    {
        if (isCooked || image == null) return;
        isCooked = true;
        image.sprite = cookedFish;
        Debug.Log("🍳 Cá đã chín!");
    }
}
