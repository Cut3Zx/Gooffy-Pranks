using UnityEngine;
using UnityEngine.EventSystems;

public class CupboardFix : BaseObjectManager
{
    [Header("References")]
    public GameObject fallenCupboard;
    public GameObject fixedCupboard;

    private bool isFixed = false;

    protected override void Awake()
    {
        base.Awake();
        fallenCupboard.SetActive(true);
        fixedCupboard.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isFixed) return;

        isFixed = true;
        fallenCupboard.SetActive(false);
        fixedCupboard.SetActive(true);

        Debug.Log("🪑 Tủ đã được dựng lại!");
        SFXManager.Instance.PlaySFX("Tusach");
        if (CleanupManager.Instance != null)
            CleanupManager.Instance.AddFixedObject();
    }
}
