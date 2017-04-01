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
        patterns.MESH = (Mesh)EditorGUILayout.ObjectField(patterns.MESH, typeof(Mesh), false);
        if (!EditorUtility.IsPersistent(patterns)) patterns.AlignWithGrid();
    }
}
