using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageThemeHolder))]
public class ImageThemeHolderInspector : Editor
{
    ImageThemeHolder _myTarget;

    void OnEnable()
    {
        _myTarget = (ImageThemeHolder)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Open Theme Image Selector"))
        {
            ThemeImageSelectWindow window = EditorWindow.GetWindow<ThemeImageSelectWindow>("Theme Image Selector");
            window.Init(_myTarget);
        }
    }
}
