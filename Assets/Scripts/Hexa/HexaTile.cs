using UnityEngine;
using System.Collections;

public class HexaTile : MonoBehaviour
{
    public static float radius = 0.5f;
    public static float heightStep = 0.25f;
    public static float cWidth = radius * 0.75f;
    public static float cHeight = radius * Mathf.Sqrt(3f) / 2f;
    public static Vector3 oddOffsets = new Vector3(0, 0, cHeight);

    public HexaGridPosition hexaGridPosition;
    public int X { get { return hexaGridPosition.x; } }
    public int Y { get { return hexaGridPosition.y; } }
    public int Z { get { return hexaGridPosition.z; } }

    public Direction direction = Direction.North;

    public enum Elevation
    {
        _0,
        _0_To_1,
        _1,
        _1_To_2,
        _2
    }
    public Elevation[] elevation = { Elevation._0, Elevation._0, Elevation._0, Elevation._0, Elevation._0, Elevation._0 };

    public Elevation GetElevation(Direction direction) { return elevation[(int)direction]; }

    public enum Property
    {
        Passable,
        Blocked,
        EntryOnly,
        ExitOnly
    }
    public Property[] property = { Property.Passable, Property.Passable, Property.Passable, Property.Passable, Property.Passable, Property.Passable };

    public Property GetProperty(Direction direction) { return property[(int)direction]; }

    public bool sceneryTile = false;
    public bool attachPositionToName = true;
    public bool ignoreCombine = false;

    public Vector3 WorldPosition { get { return hexaGridPosition.WorldPosition; } }

    public void Retarget(int xOffset, int yOffset, int zOffset)
    {
        hexaGridPosition.x += xOffset;
        hexaGridPosition.y += yOffset;
        hexaGridPosition.z += zOffset;
        UpdatePosition();
        UpdateName();
    }

    public void WorldToGridPosition()
    {
        hexaGridPosition.Copy(HexaGridPosition.ConvertWorldPosition(transform.position));
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.position = WorldPosition;
        transform.rotation = Quaternion.Euler(0, HexaDirection.GetAngle(direction), 0);
    }

    public void UpdateName()
    {
        if (!attachPositionToName)
        {
            name = GetComponent<MeshFilter>().sharedMesh.name;
            return;
        }

        name = GetComponent<MeshFilter>().sharedMesh.name + " " + hexaGridPosition.ToStringSimple();
    }

    public bool HasSameValues(HexaTile other)
    {
        return hexaGridPosition.HasSameValues(other.hexaGridPosition);
    }

    public void TurnLeft()
    {
        direction = HexaDirection.TurnLeft(direction);
        RotateTile(false);
    }
    public void TurnRight()
    {
        direction = HexaDirection.TurnRight(direction);
        RotateTile(true);
    }
    private void RotateTile(bool clockwise)
    {
        Elevation[] nextElevation = new Elevation[elevation.Length];
        Property[] nextProperty = new Property[property.Length];

        if (clockwise)
        {
            for (int i = 0; i < nextElevation.Length; i++)
            {
                if (i > 0) nextElevation[i] = elevation[i - 1]; else nextElevation[i] = elevation[nextElevation.Length - 1];
                if (i > 0) nextProperty[i] = property[i - 1]; else nextProperty[i] = property[nextElevation.Length - 1];
            }
        }
        else
        {
            for (int i = nextElevation.Length - 1; i >= 0; i--)
            {
                if (i < nextElevation.Length - 1) nextElevation[i] = elevation[i + 1]; else nextElevation[i] = elevation[0];
                if (i < nextProperty.Length - 1) nextProperty[i] = property[i + 1]; else nextProperty[i] = property[0];
            }
        }

        for (int i = 0; i < nextElevation.Length; i++)
        {
            elevation[i] = nextElevation[i];
            property[i] = nextProperty[i];
        }
    }
}
