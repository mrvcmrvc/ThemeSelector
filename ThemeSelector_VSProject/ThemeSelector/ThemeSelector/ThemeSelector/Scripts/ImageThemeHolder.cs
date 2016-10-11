using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ImageThemeInfo
{
    public string ThemeName, SpritePath, SpriteName;
    public Color SpriteColor = Color.white;
}

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class ImageThemeHolder : MonoBehaviour
{
    [HideInInspector]
    public List<ImageThemeInfo> ThemeInfoList = new List<ImageThemeInfo>();

    Image _image;

    public bool EnableDebug;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if(Application.isEditor && !CompareTag(ImageThemeController.TAG_NAME))
            DestroyImmediate(this);
    }

    void OnDestroy()
    {
        if (Application.isEditor)
            return;

        _image.sprite = null;
    }

    public void SetImageAndColor(int themeType)
    {
        ImageThemeInfo appThemeInfo = ThemeInfoList[themeType];

        if (appThemeInfo == null || string.IsNullOrEmpty(appThemeInfo.SpritePath))
        {
            if (EnableDebug)
            {
                Debug.Log("Empty Sprite: NAME: " + name + " THEME: " + themeType);
            }

            return;
        }

        string resourcesRelatedPath = appThemeInfo.SpritePath.Split(new string[] { "Resources/" }, System.StringSplitOptions.None)[1];
        string ext = Path.GetExtension(resourcesRelatedPath);
        resourcesRelatedPath = resourcesRelatedPath.Replace(ext, "");

        Sprite sprite = Resources.Load<Sprite>(resourcesRelatedPath);

        if (EnableDebug)
        {
            Debug.Log("Full Path: " + appThemeInfo.SpritePath);
            Debug.Log("Resources Related Path: " + resourcesRelatedPath);
        }

        _image.sprite = sprite;
        _image.color = appThemeInfo.SpriteColor;

        _image.SetNativeSize();
    }

    public void ResetSprite()
    {
        _image.sprite = null;
    }
}
