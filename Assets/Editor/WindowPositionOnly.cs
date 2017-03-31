using UnityEditor;

public class WindowPositionOnly : EditorWindow
{
    [MenuItem("PolygonSnake/Functions/Organize Positions Only")]
    public static void ShowWindow()
    {
        EditorHelper.FindAndPushPositionsOnly();
    }
}
