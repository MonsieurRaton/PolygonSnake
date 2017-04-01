using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public static Level instance;

    public HexaTile[] tiles;

    HexaTile[,,] map;
    public int xMax;
    public int yMax;
    public int zMax;

    public bool hasLoaded;


    public HexaTile GetTileAt(HexaGridPosition pos)
    {
        if (pos.x < 0 || pos.x >= xMax || pos.y < 0 || pos.y >= yMax || pos.z < 0 || pos.z >= zMax) return null;
        if (map[pos.x, pos.y, pos.z] == null) return null;
        return map[pos.x, pos.y, pos.z];
    }


    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("Deux Level sur scène");
        }
    }

    public void Load()
    {
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        GameObject[] foundTiles = GameObject.FindGameObjectsWithTag(Tags.HexaTile);
        tiles = new HexaTile[foundTiles.Length];

        yield return new WaitForEndOfFrame();

        // Set references & Map tiles
        //-
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = foundTiles[i].GetComponent<HexaTile>();

            if (tiles[i].sceneryTile) continue;

            if (xMax <= tiles[i].X) xMax = tiles[i].X + 1;
            if (yMax <= tiles[i].Y) yMax = tiles[i].Y + 1;
            if (zMax <= tiles[i].Z) zMax = tiles[i].Z + 1;
        }

        yield return new WaitForEndOfFrame();

        // Create game grid
        //-
        map = new HexaTile[xMax, yMax, zMax];

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].sceneryTile) continue;

            map[tiles[i].X, tiles[i].Y, tiles[i].Z] = tiles[i];
        }


        yield return new WaitForEndOfFrame();

        // Prepare groups
        //-
        List<List<HexaTile>> meshesPerMat = new List<List<HexaTile>>();
        List<Material> sharedMats = new List<Material>();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].ignoreCombine) continue;

            MeshRenderer meshRenderer = tiles[i].GetComponent<MeshRenderer>();

            if (!sharedMats.Contains(meshRenderer.sharedMaterial))
            {
                sharedMats.Add(meshRenderer.sharedMaterial);
                meshesPerMat.Add(new List<HexaTile>());
            }

            int sharedMatIndex = 0;
            for (int j = 0; j < sharedMats.Count; j++) if (meshRenderer.sharedMaterial == sharedMats[j]) sharedMatIndex = j;
            meshesPerMat[sharedMatIndex].Add(tiles[i]);
        }
        Debug.Log("Unique mats = " + meshesPerMat.Count + "\n");
        
        yield return new WaitForEndOfFrame();

        // Combine meshes
        //-
        for (int mat = 0; mat < meshesPerMat.Count; mat++)
        {
            CombineInstance[] combine = new CombineInstance[meshesPerMat[mat].Count];
            MeshFilter meshFilter;

            for (int i = 0; i < meshesPerMat[mat].Count; i++)
            {
                meshFilter = meshesPerMat[mat][i].GetComponent<MeshFilter>();
                combine[i].mesh = meshFilter.sharedMesh;
                combine[i].transform = meshFilter.transform.localToWorldMatrix;

                if (meshesPerMat[mat][i].gameObject.activeSelf) meshesPerMat[mat][i].gameObject.SetActive(false);
            }

            GameObject result = new GameObject("Combine " + sharedMats[mat].name);
            result.AddComponent<MeshFilter>();
            result.AddComponent<MeshRenderer>();
            result.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            result.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            result.GetComponent<MeshRenderer>().sharedMaterial = sharedMats[mat];
            result.SetActive(true);
            result.isStatic = true;
        }

        Debug.Log("Combined. Ok.\n");

        yield return new WaitForEndOfFrame();

        hasLoaded = true;
    }
}
