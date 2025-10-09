using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this to each chicken (KFC) GameObject.
/// Supports UI clicks (IPointerClickHandler), OnMouseDown (world objects with collider)
/// and optional trigger-enter detection (e.g., player touching the object).
/// When interacted, it calls CountingChick.Instance.RegisterFound(gameObject).
/// </summary>
public class ChickItem : MonoBehaviour, IPointerClickHandler
{
    [Header("Interaction")]
    public bool enabledInteraction = true;
    public bool disableAfterFound = true; // disable GameObject after found (optional)

    [Header("Trigger options (optional)")]
    public bool registerOnTriggerEnter = false;
    public string triggerPlayerTag = "Player"; // tag to check when using trigger

    [Header("Local Events")]
    public UnityEvent onLocalFound; // local feedback (sound, animation)

    bool isFound = false;

    // For UI Image/Button: pointer click
    public void OnPointerClick(PointerEventData eventData)
    {
        TryRegisterFound();
    }

    // For world objects with collider and camera raycast
    void OnMouseDown()
    {
        TryRegisterFound();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!registerOnTriggerEnter) return;
        if (other == null) return;
        if (!string.IsNullOrEmpty(triggerPlayerTag) && !other.CompareTag(triggerPlayerTag)) return;
        TryRegisterFound();
    }

    void TryRegisterFound()
    {
        if (!enabledInteraction) return;
        if (isFound) return;

        isFound = true;

        // Local feedback
        onLocalFound?.Invoke();

        // Notify manager
        if (CountingChick.Instance != null)
        {
            CountingChick.Instance.RegisterFound(this.gameObject);
        }

        if (disableAfterFound)
        {
            // If manager also hides it, this is redundant but safe
            gameObject.SetActive(false);
        }
    }

    // Public API to mark as found (from other scripts)
    public void MarkFound()
    {
        TryRegisterFound();
    }
}
