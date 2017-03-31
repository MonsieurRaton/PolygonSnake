using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexaTile))]
public class HexaTileEditor : Editor
{
    HexaTile tile;
    Rect rect;

    void OnEnable()
    {
        tile = (HexaTile)target;

        if (!EditorUtility.IsPersistent(tile) &&
            PrefabUtility.GetPrefabType(tile) != PrefabType.None &&
            PrefabUtility.GetPrefabType(tile) != PrefabType.DisconnectedPrefabInstance)
        {
            PrefabUtility.DisconnectPrefabInstance(tile);
        }
    }


    public override void OnInspectorGUI()
    {
        GUILayout.Label(tile.hexaGridPosition.ToString(), EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label(tile.direction.ToString(), GUILayout.Width(80));
        if (!EditorUtility.IsPersistent(tile))
        {
            if (GUILayout.Button("<- Rotate", GUILayout.Width(70))) { tile.TurnLeft(); }
            if (GUILayout.Button("Rotate ->", GUILayout.Width(70))) { tile.TurnRight(); }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("----- Elevation:");
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.North] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.North], GUILayout.Width(80));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.NorthWest] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.NorthWest], GUILayout.Width(80));
        GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.NorthEast] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.NorthEast], GUILayout.Width(80));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.SouthWest] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.SouthWest], GUILayout.Width(80));
        GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.SouthEast] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.SouthEast], GUILayout.Width(80));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.elevation[(int)Direction.South] = (HexaTile.Elevation)EditorGUILayout.EnumPopup(tile.elevation[(int)Direction.South], GUILayout.Width(80));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

        GUILayout.Label("----- Property:");
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.property[(int)Direction.North] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.North], GUILayout.Width(120));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.property[(int)Direction.NorthWest] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.NorthWest], GUILayout.Width(120));
        GUILayout.FlexibleSpace();
        tile.property[(int)Direction.NorthEast] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.NorthEast], GUILayout.Width(120));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.property[(int)Direction.SouthWest] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.SouthWest], GUILayout.Width(120));
        GUILayout.FlexibleSpace();
        tile.property[(int)Direction.SouthEast] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.SouthEast], GUILayout.Width(120));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        tile.property[(int)Direction.South] = (HexaTile.Property)EditorGUILayout.EnumPopup(tile.property[(int)Direction.South], GUILayout.Width(120));
        GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        tile.sceneryTile = EditorGUILayout.Toggle(tile.sceneryTile, GUILayout.Width(20));
        GUILayout.Label("Scenery Tile");
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        tile.ignoreCombine = EditorGUILayout.Toggle(tile.ignoreCombine, GUILayout.Width(20));
        GUILayout.Label("Don't combine");
        GUILayout.EndHorizontal();

        if (!EditorUtility.IsPersistent(tile))
        {
            GUILayout.BeginHorizontal();
            tile.attachPositionToName = EditorGUILayout.Toggle(tile.attachPositionToName, GUILayout.Width(20));
            GUILayout.Label("Attach position to name");
            GUILayout.EndHorizontal();

            tile.WorldToGridPosition();
            tile.UpdateName();

            if (GUI.changed) EditorHelper.MarkSceneDirty();
        }
    }


}
