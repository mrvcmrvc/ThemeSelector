using UnityEditor;
using UnityEngine;

public class TagChecker : Editor
{
    [InitializeOnLoadMethod]
    public static void InitUpdate()
    {
        Debug.Log("Loading Theme Selector Tag Checker");

        AddTagToTagManager();

        EditorApplication.update += CheckTags;
    }

    static void AddTagToTagManager()
    {
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // First check if it is not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(ImageThemeController.TAG_NAME)) { found = true; break; }
        }

        // if not found, add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = ImageThemeController.TAG_NAME;
        }

        // and to save the changes
        tagManager.ApplyModifiedProperties();
    }

    static void CheckTags()
    {
        var targetObjs = GameObject.FindGameObjectsWithTag(ImageThemeController.TAG_NAME);

        foreach(var obj in targetObjs)
        {
            if (obj.GetComponent<ImageThemeHolder>() == null)
                obj.AddComponent<ImageThemeHolder>();
        }
    }
}
