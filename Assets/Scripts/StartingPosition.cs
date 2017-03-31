using UnityEngine;
using System.Collections.Generic;

public class StartingPosition : MonoBehaviour
{
    public HexaGridPosition position;
    public Direction direction;

    public List<SpawnPosition> spawnPositions;
    [System.Serializable]
    public class SpawnPosition
    {
        public bool active = true;
        public bool debug = false;
        public HexaGridPosition hexaGridPosition;
        public Direction direction;

        public SpawnPosition(HexaGridPosition position, Direction direction)
        {
            hexaGridPosition = new HexaGridPosition(position.x, position.y, position.z);
            this.direction = direction;
        }
    }
    
    public Mesh visualMesh;
    public float visualMeshScale = 1;

    public Mesh Mesh { get { return GetComponent<MeshFilter>().sharedMesh; } }


    public int selected = -1;
    public void MoveNextSpawnPosition()
    {
        selected++;
        if (selected >= spawnPositions.Count) selected = 0;
    }
    public SpawnPosition GetSpawnPosition()
    {
        return spawnPositions[selected];
    }


    public void RefreshPosition()
    {
        position.Copy(HexaGridPosition.ConvertWorldPosition(transform.position));
        transform.position = position.WorldPosition;
        transform.rotation = Quaternion.Euler(0, HexaDirection.GetAngle(direction), 0);
        name = "Starting Points";
    }

    public void AddCurrentPosition()
    {
        spawnPositions.Add(new SpawnPosition(position, direction));
    }
    public void RemovePositionAt(int index)
    {
        spawnPositions.RemoveAt(index);
    }

    public void TurnLeft()
    {
        direction = HexaDirection.TurnLeft(direction);
    }

    public void TurnRight()
    {
        direction = HexaDirection.TurnRight(direction);
    }


    void OnDrawGizmos()
    {
        if (position == null) return;

        Gizmos.color = Color.gray;
        Gizmos.DrawMesh(Mesh, position.WorldPosition + Vector3.up * 0.05f, HexaDirection.GetRotation(direction), Vector3.one * 0.80f);

        if (visualMesh == null) return;

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!spawnPositions[i].active) continue;

            Gizmos.color = spawnPositions[i].debug ? Color.blue : Color.green;
            Gizmos.DrawMesh(
                visualMesh,
                spawnPositions[i].hexaGridPosition.WorldPosition + Vector3.up * 0.05f,
                HexaDirection.GetRotation(spawnPositions[i].direction),
                Vector3.one * visualMeshScale);
        }
    }
}
