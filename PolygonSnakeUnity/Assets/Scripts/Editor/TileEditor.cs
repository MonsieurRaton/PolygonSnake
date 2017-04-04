using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Tile tile = (Tile)target;

        tile.transform.position = Utility.PosistionToGrid(tile.transform.position);
        EditorGUILayout.LabelField("Position : X:" + tile.transform.position.x +
                                    " Y:" + tile.transform.position.y +
                                    " Z:" + tile.transform.position.z
                                    );

        if (tile.gameObject.isStatic == false) {
            Debug.LogWarning("Une tile n'est pas static");
        }

    }

}
