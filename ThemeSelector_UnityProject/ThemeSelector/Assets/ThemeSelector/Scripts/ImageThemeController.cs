using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ImageThemeController : MonoBehaviour
{
    public static string TAG_NAME = "ThemeChangeAffected";

    public static ImageThemeController Instance { get; private set; }

    List<ImageThemeHolder> _themeHolderList;

    IEnumerator _changeThemeRoutine;

    void Awake()
    {
        Instance = this;

        _changeThemeRoutine = null;
    }

    void Start()
    {
        if (_themeHolderList == null || _themeHolderList.Count == 0)
            FindAllThemeHolders();
    }

    void FindAllThemeHolders()
    {
        _themeHolderList = new List<ImageThemeHolder>();

        _themeHolderList = GameObject.FindObjectsOfType<ImageThemeHolder>().ToList();
    }

    public void InitImageThemes(int themeType)
    {
        if (_themeHolderList == null || _themeHolderList.Count == 0)
        {
            Debug.Log("Image Theme Holder scripts are not found.");

            return;
        }

        if (_changeThemeRoutine != null)
        {
            StopCoroutine(_changeThemeRoutine);
            _changeThemeRoutine = null;
        }

        _changeThemeRoutine = ChangeImageThemes(themeType);
        StartCoroutine(_changeThemeRoutine);
    }

    IEnumerator ChangeImageThemes(int themeType)
    {
        _themeHolderList.ForEach(h => h.ResetSprite());

        yield return null;

        Resources.UnloadUnusedAssets();

        yield return null;

        _themeHolderList.ForEach(h => h.SetImageAndColor(themeType));
    }
}
