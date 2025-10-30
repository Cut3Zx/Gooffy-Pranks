using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrackedEggClick : BaseObjectManager
{
    [Header("Kiểu trứng")]
    public bool revealsChick = false;

    [Header("Thành phần")]
    public GameObject crackTop;
    public GameObject crackBottom;
    public GameObject chickGO;
    public Sprite boiledSprite; // trứng chín
    public AudioSource sfx;

    private bool opened = false;
    private Image uiImage;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (chickGO != null)
            chickGO.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (opened) return;
        opened = true;

        OpenEgg();
    }

    private void OpenEgg()
    {
        // Ẩn crack
        if (crackTop != null) crackTop.SetActive(false);
        if (crackBottom != null)
        {
            SFXManager.Instance.PlaySFX("Ga");
            crackBottom.SetActive(false);
        }

        if (revealsChick)
        {
            if (chickGO != null)
            {
                chickGO.SetActive(true);
                Debug.Log("🐣 Gà con xuất hiện!");
            }
        }
        else
        {
            if (boiledSprite != null)
            {
                if (uiImage != null) uiImage.sprite = boiledSprite;
                if (spriteRenderer != null) spriteRenderer.sprite = boiledSprite;
                Debug.Log("🍳 Trứng chín!");
            }
        }

        if (sfx != null) sfx.Play();
    }

    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
