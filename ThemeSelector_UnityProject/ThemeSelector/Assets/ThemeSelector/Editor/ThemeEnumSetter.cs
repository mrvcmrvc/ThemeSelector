using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ThemeEnumSetter : EditorWindow
{
    static List<string> _themeTypeList;
    static bool _fileAlreadySaved;

    MonoScript _themeEnumScript = null;

    [MenuItem("Theme Selector/Theme Enum Setter")]
    static void OpenThemeEnumSetter()
    {
        _fileAlreadySaved = false;

        CheckSavedFile();

        GetWindow<ThemeEnumSetter>();
    }

    static void CheckSavedFile()
    {
        _themeTypeList = new List<string>();

        if(File.Exists(ThemeSelectorUtilities.SavedFilePath))
        {
            string content = File.ReadAllText(ThemeSelectorUtilities.SavedFilePath);
            string[] values = content.Split(new char[] { ',' });

            foreach(var value in values)
                _themeTypeList.Add(value);
        }
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        _themeEnumScript = (MonoScript)EditorGUILayout.ObjectField("Theme Enum Script", _themeEnumScript, typeof(MonoScript), false);

        if (EditorGUI.EndChangeCheck())
            _fileAlreadySaved = false;

        EditorGUILayout.Space();

        if(_themeEnumScript != null && !_fileAlreadySaved)
        {
            _fileAlreadySaved = true;

            string wholeContent = _themeEnumScript.text;
            string splitString = _themeEnumScript.name;
            string[] splitContent = wholeContent.Split(new string[] { splitString }, StringSplitOptions.RemoveEmptyEntries);

            string targetContent = splitContent[splitContent.Length - 1];
            targetContent = targetContent.Trim();
            targetContent = targetContent.Trim(new char[] { '{', '}', ' ' });

            string[] values = targetContent.Split(new char[] { ',' });

            _themeTypeList = new List<string>();
            for (int i = 0; i < values.Length; i++)
                _themeTypeList.Add(values[i].Trim());

            string content = "";

            for (int i = 0; i < _themeTypeList.Count; i++)
            {
                content += _themeTypeList[i].ToString();

                if (i != _themeTypeList.Count - 1)
                {
                    content += ",";
                }
            }

            string dirPath = Path.GetDirectoryName(ThemeSelectorUtilities.SavedFilePath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.WriteAllText(ThemeSelectorUtilities.SavedFilePath, content);
            UpdateHoldersThemes();
            AssetDatabase.Refresh();
        }

        if (_themeTypeList.Count != 0)
        {
            for (int i = 0; i < _themeTypeList.Count; i++)
                EditorGUILayout.LabelField(_themeTypeList[i]);
        }
        else
            EditorGUILayout.LabelField("Nothing to show...");
    }

    public static void UpdateHolderThemes(ImageThemeHolder imageHolder)
    {
        if (_themeTypeList == null || _themeTypeList.Count == 0)
            CheckSavedFile();

        List<ImageThemeInfo> tempList = new List<ImageThemeInfo>(imageHolder.ThemeInfoList);

        //Removal of old themes
        foreach (var item in tempList)
        {
            if (!_themeTypeList.Contains(item.ThemeName))
                imageHolder.ThemeInfoList.Remove(item);
        }

        //Adding newly created themes
        foreach (var themeName in _themeTypeList)
        {
            if (imageHolder.ThemeInfoList.Find(t => t.ThemeName == themeName) != null)
                continue;

            ImageThemeInfo newThemeImageHolder = new ImageThemeInfo();
            newThemeImageHolder.ThemeName = themeName;
            newThemeImageHolder.SpritePath = "";
            newThemeImageHolder.SpriteColor = Color.white;

            imageHolder.ThemeInfoList.Add(newThemeImageHolder);
        }
    }

    [MenuItem("Theme Selector/Update All Theme Holders/Themes")]
    static void UpdateHoldersThemes()
    {
        List<ImageThemeHolder> themeHolders = GetAllThemeHoldersInScene();

        themeHolders.ForEach(t => UpdateHolderThemes(t));
    }

    [MenuItem("Theme Selector/Update All Theme Holders/Sprite Paths")]
    static void UpdateHoldersSpritePath()
    {
        List<ImageThemeHolder> themeHolders = GetAllThemeHoldersInScene();

        foreach(var holder in themeHolders)
        {
            foreach(var info in holder.ThemeInfoList)
            {
                string newPath = UpdateThemeHolderSpritePath(info);

                info.SpritePath = newPath;
            }
        }
    }

    public static GUIContent GetButtonContent(ImageThemeInfo holder, Sprite defaultSprite)
    {
        GUIContent buttonContent = new GUIContent(defaultSprite.texture);

        if (!string.IsNullOrEmpty(holder.SpritePath))
        {
            Sprite targetSprite = AssetDatabase.LoadAssetAtPath<Sprite>(holder.SpritePath);

            if (targetSprite != null)
                buttonContent = new GUIContent(targetSprite.texture);
        }

        return buttonContent;
    }

    public static string UpdateThemeHolderSpritePath(ImageThemeInfo holder)
    {
        string newAssetPath = holder.SpritePath;

        if (!string.IsNullOrEmpty(holder.SpritePath))
        {
            Sprite targetSprite = AssetDatabase.LoadAssetAtPath<Sprite>(holder.SpritePath);

            if (targetSprite == null)
            {
                string[] guids = AssetDatabase.FindAssets(holder.SpriteName + " t:Sprite");

                if (guids.Length > 0)
                {
                    string targetGuid = guids[0];

                    newAssetPath = AssetDatabase.GUIDToAssetPath(targetGuid);
                }
            }
        }

        return newAssetPath;
    }

    static List<ImageThemeHolder> GetAllThemeHoldersInScene()
    {
        return FindObjectsOfType<ImageThemeHolder>().ToList();
    }
}
