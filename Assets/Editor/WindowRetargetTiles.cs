using UnityEditor;

public class WindowRetargetTiles : EditorWindow
{
    [MenuItem("PolygonSnake/Functions/Retarget Tiles")]
    public static void ShowWindow()
    {
        EditorHelper.FindAndRetargetTiles();
    }
}
