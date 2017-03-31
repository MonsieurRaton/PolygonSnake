using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WindowFindDoublons : EditorWindow
{
    [MenuItem("PolygonSnake/Functions/Find Doublons")]
    public static void ShowWindow()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(Tags.HexaTile);
        List<HexaTile> hexaTiles = new List<HexaTile>();

        for (int i = 0; i < tiles.Length; i++)
        {
            HexaTile ht = tiles[i].GetComponent<HexaTile>();
            if (ht)
            {
                if (hexaTiles.Count > 0)
                {
                    for (int j = 0; j < hexaTiles.Count; j++)
                    {
                        if (ht.HasSameValues(hexaTiles[j]))
                        {
                            Selection.activeGameObject = ht.gameObject;
                            Debug.Log("Doublon found : " + ht.ToString() + "\n");
                            return;
                        }
                    }
                }
                hexaTiles.Add(ht);
            }
        }

        Debug.Log("No doublons\n");
    }
}
