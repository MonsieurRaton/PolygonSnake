using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo : MonoBehaviour, IItemSimpleEvent
{
    public HexaGridPosition position;

    public List<SpawnPosition> spawnPositions;
    [System.Serializable]
    public class SpawnPosition
    {
        public bool active = true;
        public bool debug = false;
        public HexaGridPosition hexaGridPosition;

        public SpawnPosition(HexaGridPosition position)
        {
            hexaGridPosition = new HexaGridPosition(position.x, position.y, position.z);
        }

        public Vector3 WorldPosition { get { return hexaGridPosition.WorldPosition; } }
    }

    public Mesh visualMesh;
    public float visualMeshScale = 1;

    public Mesh Mesh { get { return GetComponent<MeshFilter>().sharedMesh; } }


    public void RefreshPosition()
    {
        position.Copy(HexaGridPosition.ConvertWorldPosition(transform.position));
        transform.position = position.WorldPosition;
        name = "Combo";
    }

    public void AddCurrentPosition()
    {
        spawnPositions.Add(new SpawnPosition(position));
    }
    public void RemovePositionAt(int index)
    {
        spawnPositions.RemoveAt(index);
    }


    public Item prefabStart;
    public Item prefabItem;
    Item[] bonusCache;
    int chainIndex;
    void Awake()
    {
        bonusCache = new Item[spawnPositions.Count];
        for (int i = 0; i < bonusCache.Length; i++)
        {
            bonusCache[i] = (Item)Instantiate(i == 0 ? prefabStart : prefabItem, spawnPositions[i].WorldPosition, Quaternion.identity);
            bonusCache[i].ItemSimpleEvent = this;
        }
    }

    public void StartCombo()
    {
        chainIndex = 0;
        bonusCache[chainIndex].Spawn();
    }

    IEnumerator SpawnChain()
    {
        for (int i = 1; i < bonusCache.Length; i++)
        {
            if (chainIndex < 0) yield break;

            bonusCache[i].Spawn();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void OnItemSpawn(Item item)
    {
    }

    public void OnItemPicked(Item item)
    {
        if (bonusCache[chainIndex] != item)
        {
            Debug.Log("Combo Missed\n");
            for (int i = chainIndex; i < bonusCache.Length; i++) bonusCache[i].Kill();
            chainIndex = -1;
            return;
        }

        if (chainIndex == 0) StartCoroutine(SpawnChain());

        chainIndex++;
    }

    public void OnItemDie(Item item)
    {
    }

    public void OnItemDisable(Item item)
    {
    }




    void OnDrawGizmos()
    {
        if (position == null) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawMesh(Mesh, position.WorldPosition + Vector3.up * 0.05f, Quaternion.identity, Vector3.one * 0.80f);

        if (visualMesh == null) return;

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnPositions[i].active) continue;

            Gizmos.color = spawnPositions[i].debug ? Color.blue : Color.yellow;
            Gizmos.DrawWireMesh(visualMesh, spawnPositions[i].WorldPosition + Vector3.up * 0.05f, Quaternion.identity, Vector3.one * visualMeshScale);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(spawnPositions[i].WorldPosition + Vector3.one * 0.35f, i.ToString(), UnityEditor.EditorStyles.boldLabel);
#endif
        }
    }
}
