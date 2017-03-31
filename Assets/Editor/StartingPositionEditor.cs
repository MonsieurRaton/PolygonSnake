using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StartingPosition))]
public class StartingPositionEditor : Editor
{
    StartingPosition startingPosition;


    void OnEnable()
    {
        startingPosition = (StartingPosition)target;

        if (!EditorUtility.IsPersistent(startingPosition) &&
            PrefabUtility.GetPrefabType(startingPosition) != PrefabType.None &&
            PrefabUtility.GetPrefabType(startingPosition) != PrefabType.DisconnectedPrefabInstance)
        {
            PrefabUtility.DisconnectPrefabInstance(startingPosition);
        }
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Label(startingPosition.position.ToString(), EditorStyles.boldLabel);
        GUILayout.Label(startingPosition.direction.ToString(), EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("<- Rotate", GUILayout.Width(90))) startingPosition.TurnLeft();
        if (GUILayout.Button("Rotate ->", GUILayout.Width(90))) startingPosition.TurnRight();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (startingPosition.spawnPositions == null) return;
        if (GUILayout.Button("New Starting Point")) { startingPosition.AddCurrentPosition(); }

        GUILayout.Space(5);
        GUILayout.Label("Positions :", EditorStyles.boldLabel);
        for (int i = 0; i < startingPosition.spawnPositions.Count; i++)
        {
            if (i >= startingPosition.spawnPositions.Count) return;//security event override check
            if (startingPosition.spawnPositions[i] == null) return;//security event override check

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(startingPosition.spawnPositions[i].active ? "Hide" : "Show", GUILayout.Width(40)))
            {
                startingPosition.spawnPositions[i].active = !startingPosition.spawnPositions[i].active;
            }

            if (GUILayout.Button(startingPosition.spawnPositions[i].debug ? "Norm" : "Dbg", GUILayout.Width(40)))
            {
                startingPosition.spawnPositions[i].debug = !startingPosition.spawnPositions[i].debug;
            }

            GUI.enabled = startingPosition.spawnPositions[i].active;
            GUILayout.Label(startingPosition.spawnPositions[i].hexaGridPosition.ToStringSimple(), GUILayout.Width(72));
            GUILayout.Label(startingPosition.spawnPositions[i].direction.ToString(), GUILayout.Width(60));
            GUI.enabled = true;

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                startingPosition.RemovePositionAt(i);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Label("-----------------------------------", EditorStyles.boldLabel);
        GUILayout.Label("Editor Options :", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Visual Mesh", GUILayout.Width(80));
        startingPosition.visualMesh = (Mesh)EditorGUILayout.ObjectField(startingPosition.visualMesh, typeof(Mesh), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scale", GUILayout.Width(80));
        startingPosition.visualMeshScale = EditorGUILayout.FloatField(startingPosition.visualMeshScale);
        GUILayout.EndHorizontal();

        if (!EditorUtility.IsPersistent(startingPosition))
        {
            startingPosition.RefreshPosition();
            if (GUI.changed) EditorHelper.MarkSceneDirty();
        }
    }
}
