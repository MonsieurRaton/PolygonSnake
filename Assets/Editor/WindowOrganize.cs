using UnityEditor;

public class WindowOrganize : EditorWindow
{
    [MenuItem("PolygonSnake/Functions/Organize Tiles")]
    public static void ShowWindow()
    {
        EditorHelper.FindAndOrganizeTiles();
    }
}
