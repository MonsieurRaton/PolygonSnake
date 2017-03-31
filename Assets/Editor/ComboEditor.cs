using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Combo))]
public class ComboEditor : Editor
{
    Combo combo;


    void OnEnable()
    {
        combo = (Combo)target;

        if (!EditorUtility.IsPersistent(combo) &&
            PrefabUtility.GetPrefabType(combo) != PrefabType.None &&
            PrefabUtility.GetPrefabType(combo) != PrefabType.DisconnectedPrefabInstance)
        {
            PrefabUtility.DisconnectPrefabInstance(combo);
        }
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Label(combo.position.ToString(), EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Bonus item (Start)", GUILayout.Width(120));
        combo.prefabStart = (Item)EditorGUILayout.ObjectField(combo.prefabStart, typeof(Item), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Bonus item (Chain)", GUILayout.Width(120));
        combo.prefabItem = (Item)EditorGUILayout.ObjectField(combo.prefabItem, typeof(Item), false);
        GUILayout.EndHorizontal();

        if (combo.spawnPositions == null) return;
        if (GUILayout.Button("New Starting Point")) { combo.AddCurrentPosition(); }

        GUILayout.Space(5);
        GUILayout.Label("Positions :", EditorStyles.boldLabel);
        for (int i = 0; i < combo.spawnPositions.Count; i++)
        {
            if (i >= combo.spawnPositions.Count) return;//security event override check
            if (combo.spawnPositions[i] == null) return;//security event override check

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(combo.spawnPositions[i].active ? "Hide" : "Show", GUILayout.Width(50)))
            {
                combo.spawnPositions[i].active = !combo.spawnPositions[i].active;
            }

            if (GUILayout.Button(combo.spawnPositions[i].debug ? "Normal" : "Debug", GUILayout.Width(55)))
            {
                combo.spawnPositions[i].debug = !combo.spawnPositions[i].debug;
            }

            GUI.enabled = combo.spawnPositions[i].active;
            GUILayout.Label(combo.spawnPositions[i].hexaGridPosition.ToStringSimple(), GUILayout.Width(90));
            GUI.enabled = true;

            if (GUILayout.Button("[X]", GUILayout.Width(30)))
            {
                combo.RemovePositionAt(i);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Label("-----------------------------------", EditorStyles.boldLabel);
        GUILayout.Label("Editor Options :", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Visual Mesh", GUILayout.Width(80));
        combo.visualMesh = (Mesh)EditorGUILayout.ObjectField(combo.visualMesh, typeof(Mesh), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scale", GUILayout.Width(80));
        combo.visualMeshScale = EditorGUILayout.FloatField(combo.visualMeshScale);
        GUILayout.EndHorizontal();

        if (!EditorUtility.IsPersistent(combo))
        {
            combo.RefreshPosition();
            if (GUI.changed) EditorHelper.MarkSceneDirty();
        }
    }
}
