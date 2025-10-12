using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    bool active = false;

    public void ChangeLanguage(int index)
    {
        if (active) return;
        StartCoroutine(SetLanguage(index));
    }

    IEnumerator SetLanguage(int index)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        Debug.Log($"✅ Language changed to: {LocalizationSettings.SelectedLocale.LocaleName}");
        PlayerPrefs.SetInt("LanguageIndex", index);
        active = false;
    }

    void Start()
    {
        int savedLang = PlayerPrefs.GetInt("LanguageIndex", 0);
        StartCoroutine(SetLanguage(savedLang));
    }
}
