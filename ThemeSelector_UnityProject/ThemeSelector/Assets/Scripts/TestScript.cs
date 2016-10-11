using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void ChangeTheme1()
    {
        ImageThemeController.Instance.InitImageThemes((int)Themes.Theme1);
    }

    public void ChangeTheme2()
    {
        ImageThemeController.Instance.InitImageThemes((int)Themes.Theme2);
    }

    public void ChangeTheme3()
    {
        ImageThemeController.Instance.InitImageThemes((int)Themes.Theme3);
    }
}
