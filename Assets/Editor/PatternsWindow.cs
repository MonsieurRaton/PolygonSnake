using UnityEngine;
using UnityEditor;
using System.Collections;

public class PatternsWindow : EditorWindow
{
    Material dropMaterial = null;
    bool enable = false;
    HexaTile hexaTile = null;

    [MenuItem("PolygonSnake/Patterns")]
    static void Init()
    {
        PatternsWindow window = (PatternsWindow)GetWindow(typeof(PatternsWindow));
        window.Show();
    }

    void OnGUI()
    {
        // Selection
        //---------------------------------------------------------------------------------------------
        GameObject selected = Selection.activeGameObject;
        Patterns pattern = selected != null ? selected.GetComponent<Patterns>() : null;
        if (pattern == null)
        {
            EditorGUILayout.HelpBox("Needs a Pattern gameObject", MessageType.Warning);
            return;
        }
        if (EditorUtility.IsPersistent(pattern))
        {
            EditorGUILayout.HelpBox("This is a prefab! Select a scene object instead.", MessageType.Error);
            return;
        }

        GUILayout.Label("HexaTile to instantiate");
        hexaTile = (HexaTile)EditorGUILayout.ObjectField(hexaTile, typeof(HexaTile), false);

        GUILayout.Label("Material selector (Use the selector to add new materials)");
        dropMaterial = (Material)EditorGUILayout.ObjectField(dropMaterial, typeof(Material), false);

        if (dropMaterial != null)
        {
            pattern.AddMaterial(dropMaterial);
            dropMaterial = null;
        }

        if (pattern.MaterialCount == 0)
        {
            EditorGUILayout.HelpBox("Need at least one material", MessageType.Warning);
            return;
        }

        EditorGUILayout.Separator();
        GUILayout.Label("Starting point :" + pattern.hexaGridPosition.ToString(), EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        // Values
        //---------------------------------------------------------------------------------------------
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern", EditorStyles.boldLabel, GUILayout.Width(100));
        pattern.pattern = (Patterns.Pattern)EditorGUILayout.EnumPopup(pattern.pattern);
        GUILayout.EndHorizontal();

        if (pattern.pattern == Patterns.Pattern.SimpleGrid || pattern.pattern == Patterns.Pattern.HexaGrid)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Direction", EditorStyles.boldLabel, GUILayout.Width(100));
            pattern.direction = (Direction)EditorGUILayout.EnumPopup(pattern.direction);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Width", GUILayout.Width(60));
            pattern.width = EditorGUILayout.IntSlider(pattern.width, 1, 100);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Height", GUILayout.Width(60));
            pattern.height = EditorGUILayout.IntSlider(pattern.height, 1, 100);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            pattern.fillCenter = EditorGUILayout.Toggle(pattern.fillCenter, GUILayout.Width(30));
            GUILayout.Label("Fill Center", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }

        if (pattern.pattern == Patterns.Pattern.Circle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Size", EditorStyles.boldLabel, GUILayout.Width(60));
            pattern.size = EditorGUILayout.IntSlider(pattern.size, 1, 100);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            pattern.fillCenter = EditorGUILayout.Toggle(pattern.fillCenter, GUILayout.Width(30));
            GUILayout.Label("Fill Center", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }


        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Direction", EditorStyles.boldLabel, GUILayout.Width(60));
        //pattern.direction = (Direction)EditorGUILayout.EnumPopup(pattern.direction);
        //GUILayout.EndHorizontal();

        EditorGUILayout.Separator();


        // OK
        //---------------------------------------------------------------------------------------------
        enable = pattern.HasActiveMaterials;

        if (hexaTile == null)
        {
            EditorGUILayout.HelpBox("Needs an HexaTile to copy", MessageType.Error);
            enable = false;
        }

        GUILayout.BeginHorizontal();
        GUI.enabled = enable;

        if (GUILayout.Button("--- CREATE ---", GUILayout.Height(30)))
        {
            pattern.Create(hexaTile);
            return;
        }
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        // Materials
        //---------------------------------------------------------------------------------------------
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mats per line", EditorStyles.boldLabel, GUILayout.Width(100));
        pattern.matsPerLine = EditorGUILayout.IntSlider(pattern.matsPerLine, 4, 10);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        for (int i = 0; i < pattern.MaterialCount; i++)
        {
            if (i >= pattern.MaterialCount) return;//security event override check
            if (pattern.GetPatternMaterial(i) == null) return;//security event override check

            if (i > 0 && i % pattern.matsPerLine == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(pattern.GetPatternMaterial(i).inUse ? "Disable" : "Enable", GUILayout.Width(60)))
            {
                pattern.GetPatternMaterial(i).inUse = !pattern.GetPatternMaterial(i).inUse;
            }
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                pattern.RemoveMaterialAt(i);
                return;
            }
            GUILayout.EndHorizontal();

            GUI.enabled = pattern.GetPatternMaterial(i).inUse;
            GUILayout.Label(AssetPreview.GetAssetPreview(pattern.GetPatternMaterial(i).material), GUILayout.Width(80), GUILayout.Height(80));
            GUI.enabled = true;
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(pattern);
    }

    void OnSelectionChange()
    {
        Repaint();
    }
}
