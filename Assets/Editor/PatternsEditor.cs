using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Patterns))]
public class PatternsEditor : Editor
{
    Patterns patterns;

    void OnEnable()
    {
        patterns = (Patterns)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label(patterns.hexaGridPosition.ToString(), EditorStyles.largeLabel);

        if (!EditorUtility.IsPersistent(patterns)) patterns.AlignWithGrid();
    }
}
