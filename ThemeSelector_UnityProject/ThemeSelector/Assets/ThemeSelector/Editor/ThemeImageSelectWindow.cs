using UnityEngine;
using UnityEditor;
using System.IO;

public class ThemeImageSelectWindow : EditorWindow
{
    ImageThemeHolder _myTarget;
    GUIStyle newLabelStyle, newImageButtonStyle;
    Sprite AddTexture;

    const float _guiWidth = 256f;

    string _selectedTheme;

    public void Init(ImageThemeHolder imageHolder)
    {
        AddTexture = (Sprite)AssetDatabase.LoadAssetAtPath(ThemeSelectorUtilities.AddTexturePath, typeof(Sprite));

        _myTarget = imageHolder;

        newLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
        newLabelStyle.imagePosition = ImagePosition.TextOnly;
        newLabelStyle.normal.textColor = Color.black;
        newLabelStyle.fontStyle = FontStyle.Bold;
        newLabelStyle.alignment = TextAnchor.MiddleCenter;
        newLabelStyle.fixedWidth = _guiWidth;

        newImageButtonStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
        newImageButtonStyle.imagePosition = ImagePosition.ImageOnly;
        newImageButtonStyle.alignment = TextAnchor.MiddleCenter;
        newImageButtonStyle.fixedWidth = _guiWidth;
        newImageButtonStyle.fixedHeight = _guiWidth;

        ThemeEnumSetter.UpdateHolderThemes(imageHolder);
    }

    void OnDisable()
    {
        _myTarget = null;

        newLabelStyle = null;
        newImageButtonStyle = null;
    }

    void OnGUI()
    {
        if(AddTexture == null || _myTarget == null)
        {
            EditorGUILayout.LabelField("Nothing to show..."); //TODO: Burası ekranda gözükmüyor
            return;
        }

        EditorGUILayout.BeginHorizontal();

        foreach(var holder in _myTarget.ThemeInfoList)
        {
            Rect rect = EditorGUILayout.BeginVertical();

            EditorGUI.DrawRect(rect, Color.grey);

            EditorGUILayout.LabelField(holder.ThemeName, newLabelStyle);

            holder.SpritePath = ThemeEnumSetter.UpdateThemeHolderSpritePath(holder);

            GUIContent buttonContent = ThemeEnumSetter.GetButtonContent(holder, AddTexture);

            GUI.color = holder.SpriteColor;

            if(GUILayout.Button(buttonContent, newImageButtonStyle))
            {
                _selectedTheme = holder.ThemeName;

                EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "", 2);
            }

            GUI.color = Color.white;

            holder.SpriteColor = EditorGUILayout.ColorField(holder.SpriteColor, GUILayout.Width(_guiWidth));

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        CheckForSelectorUpdate();
    }

    void CheckForSelectorUpdate()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            if (!AssetDatabase.GetAssetPath(EditorGUIUtility.GetObjectPickerObject()).Contains("Resources"))
            {
                Debug.Log("Please Select a sprite from resources folder.");

                return;
            }

            Sprite selTexture = (Sprite)EditorGUIUtility.GetObjectPickerObject();

            _myTarget.ThemeInfoList.Find(h => h.ThemeName == _selectedTheme).SpritePath = AssetDatabase.GetAssetPath(selTexture);
            _myTarget.ThemeInfoList.Find(h => h.ThemeName == _selectedTheme).SpriteName = selTexture.name;

            Repaint();
        }
    }
}
