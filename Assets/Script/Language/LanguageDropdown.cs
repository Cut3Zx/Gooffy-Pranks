using UnityEngine;

public class LanguageDropdown : MonoBehaviour
{
    public GameObject buttonEnglish;
    public GameObject buttonVietnamese;

    private bool isOpen = false;

    public void ToggleLanguageOptions()
    {
        isOpen = !isOpen;
        buttonEnglish.SetActive(isOpen);
        buttonVietnamese.SetActive(isOpen);
    }

    public void HideOptions()
    {
        isOpen = false;
        buttonEnglish.SetActive(false);
        buttonVietnamese.SetActive(false);
    }
}
