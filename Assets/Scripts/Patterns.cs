using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patterns : MonoBehaviour
{
    public HexaGridPosition hexaGridPosition = new HexaGridPosition();
    public Direction direction = Direction.North;
    public int size = 1;
    public int width = 1;
    public int height = 1;
    public bool fillCenter = true;
    public int matsPerLine = 4;

    public enum Pattern
    {
        SimpleGrid,
        HexaGrid,
        Circle
    }
    public Pattern pattern = Pattern.SimpleGrid;


    public Mesh MESH { get { return GetComponent<MeshFilter>().sharedMesh; } }


    public void AlignWithGrid()
    {
        hexaGridPosition = HexaGridPosition.ConvertWorldPosition(transform.position);
        transform.position = hexaGridPosition.WorldPosition;
    }


    public List<PatternMaterial> materials = new List<PatternMaterial>();
    public List<Material> materialsRandom = new List<Material>();

    [System.Serializable]
    public class PatternMaterial
    {
        public bool inUse = true;
        public Material material;

        public PatternMaterial(Material material) { this.material = material; }
    }

    public void AddMaterial(Material material) { materials.Add(new PatternMaterial(material)); }
    public void RemoveMaterialAt(int index) { materials.RemoveAt(index); }
    public PatternMaterial GetPatternMaterial(int index) { return materials[index]; }

    public int MaterialCount { get { return materials.Count; } }
    public bool HasActiveMaterials
    {
        get
        {
            for (int i = 0; i < materials.Count; i++) if (materials[i].inUse) return true;
            return false;
        }
    }

    void MakeValidMatsRandomSelection()
    {
        materialsRandom.Clear();
        for (int i = 0; i < materials.Count; i++) if (materials[i].inUse) materialsRandom.Add(materials[i].material);
    }


    public void Create(HexaTile prefab)
    {
        if (pattern == Pattern.SimpleGrid || pattern == Pattern.HexaGrid) CreateRectangle(prefab);
        if (pattern == Pattern.Circle) CreateCircle(prefab);
    }


    void CreateRectangle(HexaTile prefab)
    {
        MakeValidMatsRandomSelection();

        HexaGridPosition current = new HexaGridPosition(hexaGridPosition);
        HexaGridPosition lastW = new HexaGridPosition(hexaGridPosition);

        for (int w = 1; w <= width; w++)
        {
            for (int h = 1; h <= height; h++)
            {
                if (fillCenter || (!fillCenter && (w == 1 || h == 1 || w == width || h == height)))
                {
                    HexaTile clone = Instantiate(prefab);
                    clone.hexaGridPosition.Copy(current);
                    clone.UpdatePosition();
                    clone.UpdateName();
                    clone.GetComponent<MeshRenderer>().sharedMaterial = materialsRandom[Random.Range(0, materialsRandom.Count)];
                }

                current.Move(HexaDirection.GetNext(direction, -1));
            }

            lastW.Move((pattern == Pattern.SimpleGrid && w % 2 == 0) ? HexaDirection.GetNext(direction, 1) : direction);
            current.Copy(lastW);
        }
    }


    void CreateCircle(HexaTile prefab)
    {
        MakeValidMatsRandomSelection();

        HexaGridPosition current = new HexaGridPosition(hexaGridPosition);
        HexaGridPosition destination = new HexaGridPosition(hexaGridPosition);
        Direction radiusDirection = Direction.SouthEast;

        for (int s = 1; s <= size; s++)
        {
            current.Move(Direction.North);
            if (!fillCenter && s < size) continue;

            for (int i = 1; i <= 6; i++)
            {
                if (i == 1) radiusDirection = Direction.SouthEast;
                if (i == 2) radiusDirection = Direction.South;
                if (i == 3) radiusDirection = Direction.SouthWest;
                if (i == 4) radiusDirection = Direction.NorthWest;
                if (i == 5) radiusDirection = Direction.North;
                if (i == 6) radiusDirection = Direction.NorthEast;

                destination.Copy(current);
                for (int dest = 1; dest <= s; dest++) destination.Move(radiusDirection);

                int securityCheck = 300;
                while (!current.HasSameValues(destination))
                {
                    securityCheck--; if (securityCheck == 0) { Debug.Log("Security break"); break; }

                    HexaTile clone = Instantiate(prefab);
                    clone.hexaGridPosition.Copy(current);
                    clone.UpdatePosition();
                    clone.UpdateName();
                    clone.GetComponent<MeshRenderer>().sharedMaterial = materialsRandom[Random.Range(0, materialsRandom.Count)];

                    current.Move(radiusDirection);
                }
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        if (pattern == Pattern.SimpleGrid || pattern == Pattern.HexaGrid)
        {
            HexaGridPosition current = new HexaGridPosition(hexaGridPosition);
            HexaGridPosition lastW = new HexaGridPosition(hexaGridPosition);

            for (int w = 1; w <= width; w++)
            {
                for (int h = 1; h <= height; h++)
                {
                    if (fillCenter || (!fillCenter && (w == 1 || h == 1 || w == width || h == height)))
                    {
                        Gizmos.DrawWireMesh(MESH, current.WorldPosition, Quaternion.identity, Vector3.one);
                    }

                    current.Move(HexaDirection.GetNext(direction, -1));
                }

                lastW.Move((pattern == Pattern.SimpleGrid && w % 2 == 0) ? HexaDirection.GetNext(direction, 1) : direction);
                current.Copy(lastW);
            }
        }

        if (pattern == Pattern.Circle)
        {
            HexaGridPosition current = new HexaGridPosition(hexaGridPosition);
            HexaGridPosition destination = new HexaGridPosition(hexaGridPosition);
            Direction radiusDirection = Direction.SouthEast;

            for (int s = 1; s <= size; s++)
            {
                current.Move(Direction.North);
                if (!fillCenter && s < size) continue;

                for (int i = 1; i <= 6; i++)
                {
                    if (i == 1) radiusDirection = Direction.SouthEast;
                    if (i == 2) radiusDirection = Direction.South;
                    if (i == 3) radiusDirection = Direction.SouthWest;
                    if (i == 4) radiusDirection = Direction.NorthWest;
                    if (i == 5) radiusDirection = Direction.North;
                    if (i == 6) radiusDirection = Direction.NorthEast;

                    destination.Copy(current);
                    for (int dest = 1; dest <= s; dest++) destination.Move(radiusDirection);

                    int securityCheck = 300;
                    while (!current.HasSameValues(destination))
                    {
                        securityCheck--; if (securityCheck == 0) { Debug.Log("Security break"); break; }

                        Gizmos.DrawWireMesh(MESH, current.WorldPosition, Quaternion.identity, Vector3.one);
                        current.Move(radiusDirection);
                    }
                }
            }
        }

    }
}
