using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour, IItemSimpleEvent
{
    public HexaGridPosition position;

    public List<Item> items;
    public void AddItem(Item item) { items.Add(item); }
    public void RemoveItemAt(int index) { items.RemoveAt(index); }

    public float spawnDelayMin;
    public float spawnDelayMax;
    public int maxItems;

    public List<SpawnPosition> spawnPositions;
    [System.Serializable]
    public class SpawnPosition
    {
        public bool active = true;
        public bool debug = false;
        public Item inUse = null;
        public HexaGridPosition hexaGridPosition;

        public SpawnPosition(HexaGridPosition position)
        {
            hexaGridPosition = new HexaGridPosition(position.x, position.y, position.z);
        }
    }
    
    public Mesh visualMesh;
    public Vector3 visualMeshAngle;
    public float visualMeshScale = 1;

    public Mesh Mesh { get { return GetComponent<MeshFilter>().sharedMesh; } }


    public void WorldToGridPosition()
    {
        position.Copy(HexaGridPosition.ConvertWorldPosition(transform.position));
        transform.position = position.WorldPosition;
        name = "Spawner";
    }

    public void AddCurrentPosition()
    {
        spawnPositions.Add(new SpawnPosition(position));
    }
    public void RemovePositionAt(int index)
    {
        spawnPositions.RemoveAt(index);
    }


    List<Item> instantiatedItems;
    void Start()
    {
        instantiatedItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                instantiatedItems.Add(Instantiate(items[i]));
                instantiatedItems[instantiatedItems.Count - 1].ItemSimpleEvent = this;
            }
        }
    }

    float time = 0;
    int instances = 0;
    public bool Spawn { get; set; }
    void Update()
    {
        if (!Spawn) return;

        if (instances < maxItems && instantiatedItems.Count > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = Random.Range(spawnDelayMin, spawnDelayMax);

                int randItem = Random.Range(0, instantiatedItems.Count);
                int randPosition = Random.Range(0, spawnPositions.Count);

                if (!spawnPositions[randPosition].inUse)
                {
                    instances++;
                    spawnPositions[randPosition].inUse = instantiatedItems[randItem];
                    instantiatedItems[randItem].transform.position = spawnPositions[randPosition].hexaGridPosition.WorldPosition;
                    instantiatedItems[randItem].Spawn();
                    instantiatedItems.RemoveAt(randItem);
                }
            }
        }
    }

    public void OnItemSpawn(Item item)
    {

    }
    public void OnItemPicked(Item item)
    {

    }
    public void OnItemDie(Item item)
    {

    }
    public void OnItemDisable(Item item)
    {
        instantiatedItems.Add(item);
        instances--;
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (spawnPositions[i].inUse == item)
            {
                spawnPositions[i].inUse = null;
                break;
            }
        }
    }



    void OnDrawGizmos()
    {
        if (position == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawMesh(Mesh, position.WorldPosition + Vector3.up * 0.1f, Quaternion.identity, Vector3.one * 0.90f);

        if (visualMesh == null) return;

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnPositions[i].active) continue;

            Gizmos.color = spawnPositions[i].debug ? Color.blue : Color.cyan;
            Gizmos.DrawMesh(
                visualMesh,
                spawnPositions[i].hexaGridPosition.WorldPosition + Vector3.up * 0.05f,
                Quaternion.Euler(visualMeshAngle),
                Vector3.one * visualMeshScale);
        }
    }
}
