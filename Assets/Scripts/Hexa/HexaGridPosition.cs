using UnityEngine;
using System.Collections;

[System.Serializable]
public class HexaGridPosition
{
    public int x;
    public int y;
    public int z;
    public HexaGridPosition() : this(0, 0, 0) { }
    public HexaGridPosition(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }
    public HexaGridPosition(HexaGridPosition position) { x = position.x; y = position.y; z = position.z; }

    public bool HasSameValues(HexaGridPosition other)
    {
        return other.x == x && other.y == y && other.z == z;
    }

    public void Move(Direction direction) { Move(direction, 1); }
    public void Move(Direction direction, int step)
    {
        while (step >= 1)
        {
            step--;

            if (direction == Direction.North) z += 1;
            if (direction == Direction.South) z -= 1;

            if (x % 2 == 0)
            {
                if (direction == Direction.NorthEast) x += 1;
                if (direction == Direction.NorthWest) x -= 1;
                if (direction == Direction.SouthEast) { x += 1; z -= 1; }
                if (direction == Direction.SouthWest) { x -= 1; z -= 1; }
            }
            else
            {
                if (direction == Direction.NorthEast) { x += 1; z += 1; };
                if (direction == Direction.NorthWest) { x -= 1; z += 1; };
                if (direction == Direction.SouthEast) x += 1;
                if (direction == Direction.SouthWest) x -= 1;
            }
        }
    }

    public HexaGridPosition GetNext(HexaDirection hexaDirection)
    {
        if (hexaDirection.direction == Direction.North) return new HexaGridPosition(x, y, z + 1);
        if (hexaDirection.direction == Direction.South) return new HexaGridPosition(x, y, z - 1);

        if (x % 2 == 0)
        {
            if (hexaDirection.direction == Direction.NorthEast) return new HexaGridPosition(x + 1, y, z);
            if (hexaDirection.direction == Direction.NorthWest) return new HexaGridPosition(x - 1, y, z);
            if (hexaDirection.direction == Direction.SouthEast) return new HexaGridPosition(x + 1, y, z - 1);
            if (hexaDirection.direction == Direction.SouthWest) return new HexaGridPosition(x - 1, y, z - 1);
        }
        else
        {
            if (hexaDirection.direction == Direction.NorthEast) return new HexaGridPosition(x + 1, y, z + 1);
            if (hexaDirection.direction == Direction.NorthWest) return new HexaGridPosition(x - 1, y, z + 1);
            if (hexaDirection.direction == Direction.SouthEast) return new HexaGridPosition(x + 1, y, z);
            if (hexaDirection.direction == Direction.SouthWest) return new HexaGridPosition(x - 1, y, z);
        }

        return new HexaGridPosition(x, y, z);
    }

    public Vector3 WorldPosition
    {
        get
        {
            return new Vector3(x * HexaTile.cWidth * 2, y * HexaTile.heightStep, z * HexaTile.cHeight * 2 + (x % 2 == 0 ? 0 : HexaTile.cHeight));
        }
    }

    public static HexaGridPosition ConvertWorldPosition(Vector3 pos)
    {
        HexaGridPosition hgp = new HexaGridPosition();

        hgp.x = Mathf.RoundToInt(pos.x / (HexaTile.cWidth * 2));

        hgp.y = Mathf.RoundToInt(pos.y / HexaTile.heightStep);

        if (hgp.x % 2 != 0) pos.z -= HexaTile.cHeight;
        hgp.z = Mathf.RoundToInt(pos.z / (HexaTile.cHeight * 2));


        return hgp;
    }

    public void Copy(HexaGridPosition other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
    }

    public override string ToString()
    {
        return "HexaGridPosition: X: " + x + ", Y: " + y + ", Z: " + z;
    }
    public string ToStringSimple()
    {
        return "[ " + x + ", " + y + ", " + z + " ]";
    }
}
