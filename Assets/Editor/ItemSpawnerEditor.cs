using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ItemSpawner))]
public class ItemSpawnerEditor : Editor
{
    ItemSpawner spawner;
    Item item;


    void OnEnable()
    {
        spawner = (ItemSpawner)target;

        if (!EditorUtility.IsPersistent(spawner) &&
            PrefabUtility.GetPrefabType(spawner) != PrefabType.None &&
            PrefabUtility.GetPrefabType(spawner) != PrefabType.DisconnectedPrefabInstance)
        {
            PrefabUtility.DisconnectPrefabInstance(spawner);
        }
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Label(spawner.position.ToString(), EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Delay (sec):", GUILayout.Width(90));
        spawner.spawnDelayMin = EditorGUILayout.FloatField(spawner.spawnDelayMin, GUILayout.Width(50));
        GUILayout.Label("~", GUILayout.Width(15));
        spawner.spawnDelayMax = EditorGUILayout.FloatField(spawner.spawnDelayMax, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max items:", GUILayout.Width(90));
        spawner.maxItems = EditorGUILayout.IntSlider(spawner.maxItems, 1, 30, GUILayout.Width(160));
        GUILayout.EndHorizontal();

        CustomSeparator();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Item selector", GUILayout.Width(100));
        item = (Item)EditorGUILayout.ObjectField(item, typeof(Item), false);
        if (item != null)
        {
            spawner.AddItem(item);
            item = null;
        }
        GUILayout.EndHorizontal();

        if (spawner.items == null) return;

        GUILayout.BeginHorizontal();
        for (int i = 0; i < spawner.items.Count; i++)
        {
            if (i >= spawner.items.Count) return;//security event override check
            if (spawner.items[i] == null) return;//security event override check

            if (i > 0 && i % 2 == 0) { GUILayout.EndHorizontal(); GUILayout.BeginHorizontal(); }
            if (GUILayout.Button("[X] " + spawner.items[i].name, GUILayout.Width(120))) { spawner.RemoveItemAt(i); }
        }
        GUILayout.EndHorizontal();


        CustomSeparator();

        if (spawner.spawnPositions == null) return;
        if (GUILayout.Button("New Spawn Point")) { spawner.AddCurrentPosition(); }

        GUILayout.Space(5);
        GUILayout.Label("Positions :", EditorStyles.boldLabel);
        for (int i = 0; i < spawner.spawnPositions.Count; i++)
        {
            if (i >= spawner.spawnPositions.Count) return;//security event override check
            if (spawner.spawnPositions[i] == null) return;//security event override check

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(spawner.spawnPositions[i].active ? "Hide" : "Show", GUILayout.Width(50)))
            {
                spawner.spawnPositions[i].active = !spawner.spawnPositions[i].active;
            }

            if (GUILayout.Button(spawner.spawnPositions[i].debug ? "Normal" : "Debug", GUILayout.Width(55)))
            {
                spawner.spawnPositions[i].debug = !spawner.spawnPositions[i].debug;
            }

            GUI.enabled = spawner.spawnPositions[i].active;
            GUILayout.Label(spawner.spawnPositions[i].hexaGridPosition.ToStringSimple(), GUILayout.Width(90));
            GUI.enabled = true;

            if (GUILayout.Button("[X]", GUILayout.Width(30)))
            {
                spawner.RemovePositionAt(i);
            }

            GUILayout.EndHorizontal();
        }


        CustomSeparator();


        GUILayout.Label("Editor Options :", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Visual Mesh", GUILayout.Width(80));
        spawner.visualMesh = (Mesh)EditorGUILayout.ObjectField(spawner.visualMesh, typeof(Mesh), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scale", GUILayout.Width(80));
        spawner.visualMeshScale = EditorGUILayout.FloatField(spawner.visualMeshScale);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Rotation", GUILayout.Width(80));
        spawner.visualMeshAngle = EditorGUILayout.Vector3Field("", spawner.visualMeshAngle);
        GUILayout.EndHorizontal();
        
        if (!EditorUtility.IsPersistent(spawner))
        {
            spawner.WorldToGridPosition();
            if (GUI.changed) EditorHelper.MarkSceneDirty();
        }
    }

    public void CustomSeparator()
    {
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        GUILayout.Label("--------------------------", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
    }
}
