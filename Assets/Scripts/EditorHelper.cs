using UnityEngine;
using System.Collections.Generic;

public class EditorHelper : MonoBehaviour
{
    public Transform tiles;
    public Transform scenery;

    [HideInInspector]
    public HexaTile[] hexaTilesCacheArray;
    [HideInInspector]
    public List<HexaTile> hexaTilesCacheList;
    

    public static void MarkSceneDirty()
    {
#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#endif
    }


    public void CacheTilesArray()
    {
        hexaTilesCacheArray = FindHexaTiles();
    }

    public void CacheTilesList()
    {
        hexaTilesCacheList.Clear();
        GameObject[] found = GameObject.FindGameObjectsWithTag(Tags.HexaTile);
        for (int i = 0; i < found.Length; i++) hexaTilesCacheList.Add(found[i].GetComponent<HexaTile>());
    }


    public static HexaTile[] FindHexaTiles()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag(Tags.HexaTile);
        HexaTile[] tiles = new HexaTile[found.Length];
        for (int i = 0; i < tiles.Length; i++) tiles[i] = found[i].GetComponent<HexaTile>();
        return tiles;
    }

    public static EditorHelper Find()
    {
        GameObject editorHelperGameObject = GameObject.FindGameObjectWithTag(Tags.EditorHelper);
        return editorHelperGameObject == null ? null : editorHelperGameObject.GetComponent<EditorHelper>();
    }


    public static void FindAndPushPositionsOnly()
    {
        EditorHelper editorHelper = Find();
        if (editorHelper == null)
        {
            Debug.LogError("There is no [EditorHelper] in the scene\n");
            return;
        }

        editorHelper.PushPositionsOnly();
    }
    public void PushPositionsOnly()
    {
        CacheTilesArray();
        if (hexaTilesCacheArray.Length == 0) return;

        for (int i = 0; i < hexaTilesCacheArray.Length; i++) hexaTilesCacheArray[i].WorldToGridPosition();

        Debug.LogWarning("Positions have been refreshed\n");
        MarkSceneDirty();
    }

    public static void FindAndOrganizeTiles()
    {
        EditorHelper editorHelper = Find();
        if (editorHelper == null)
        {
            Debug.LogError("There is no [EditorHelper] in the scene\n");
            return;
        }

        editorHelper.OrganizeTiles();
    }
    public void OrganizeTiles()
    {
        CacheTilesArray();
        if (hexaTilesCacheArray.Length == 0) return;

        for (int i = 0; i < hexaTilesCacheArray.Length; i++)
        {
            if (hexaTilesCacheArray[i].sceneryTile && hexaTilesCacheArray[i].transform.parent != scenery) hexaTilesCacheArray[i].transform.parent = scenery;
            if (!hexaTilesCacheArray[i].sceneryTile && hexaTilesCacheArray[i].transform.parent != tiles) hexaTilesCacheArray[i].transform.parent = tiles;

            hexaTilesCacheArray[i].WorldToGridPosition();
        }

        Debug.LogWarning("All tiles have been successfully organized\n");
        MarkSceneDirty();
    }

    public static void FindAndRetargetTiles()
    {
        EditorHelper editorHelper = Find();
        if (editorHelper == null)
        {
            Debug.LogError("There is no [EditorHelper] in the scene\n");
            return;
        }

        editorHelper.RetargetTiles();
    }

    public void RetargetTiles()
    {
        CacheTilesArray();
        if (hexaTilesCacheArray.Length == 0) return;
        
        int minX = 99, minY = 99, minZ = 99;

        for (int i = 0; i < hexaTilesCacheArray.Length; i++)
        {
            if (hexaTilesCacheArray[i].sceneryTile) continue;
            if (hexaTilesCacheArray[i].X < minX) minX = hexaTilesCacheArray[i].X;
            if (hexaTilesCacheArray[i].Y < minY) minY = hexaTilesCacheArray[i].Y;
            if (hexaTilesCacheArray[i].Z < minZ) minZ = hexaTilesCacheArray[i].Z;
        }

        int offsetX = 0 - minX;
        int offsetY = 0 - minY;
        int offsetZ = 0 - minZ;

        if (offsetX == 0 && offsetY == 0 && offsetZ == 0)
        {
            Debug.LogWarning("All tiles are already aligned to a positive grid\n");
            return;
        }

        Debug.Log("--- Offsets found : " + offsetX + "," + offsetY + "," + offsetZ + "\n");

        for (int i = 0; i < hexaTilesCacheArray.Length; i++)
        {
            hexaTilesCacheArray[i].Retarget(offsetX, offsetY, offsetZ);
            hexaTilesCacheArray[i].UpdatePosition();
            hexaTilesCacheArray[i].UpdateName();
        }

        Debug.LogWarning("All tiles have been successfully pushed to fit a positive grid starting at (0,0,0)\n");
        MarkSceneDirty();
    }

}
