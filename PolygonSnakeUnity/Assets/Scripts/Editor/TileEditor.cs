using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor {


    private Tile tile;

    void OnEnable() {
        tile = (Tile)target;        
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (tile.transform.position != Utility.PosistionToGrid(tile.transform.position)) {
            tile.transform.position = Utility.PosistionToGrid(tile.transform.position);
            if (tile.gameObject.isStatic == false) {
                Debug.LogWarning("Une tile n'est pas static");
            }
        }

        EditorGUILayout.LabelField("Position : X:" + tile.transform.position.x +
                                    " Y:" + tile.transform.position.y +
                                    " Z:" + tile.transform.position.z
                                    );

    }

}
