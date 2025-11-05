#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DevTools
{
    [MenuItem("DevTools/Reset All PlayerPrefs")]
    static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("🔄 PlayerPrefs đã được reset!");
    }

    [MenuItem("DevTools/Reset Album Data")]
    static void ResetAlbumData()
    {
        // Reset tất cả level collected
        for (int i = 1; i <= 100; i++) // giả sử có tối đa 100 level
        {
            PlayerPrefs.DeleteKey($"Collected_Level_{i}");
        }
        PlayerPrefs.Save();
        Debug.Log("🔄 Dữ liệu Album đã được reset!");
    }

    [MenuItem("DevTools/Unlock All Levels")]
    static void UnlockAllLevels()
    {
        for (int i = 1; i <= 100; i++)
        {
            PlayerPrefs.SetInt($"Collected_Level_{i}", 1);
        }
        PlayerPrefs.Save();
        Debug.Log("🔓 Đã mở khóa tất cả levels!");
    }
}
#endif