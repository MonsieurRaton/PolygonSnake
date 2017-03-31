using UnityEngine;
using System.Collections;

[System.Serializable]
public class HexaDirection
{
    public Direction direction = Direction.North;

    public Direction Opposite { get { return GetOpposite(direction); } }

    public static Direction GetNext(Direction direction, int offset)
    {
        int i = (int)direction;
        i += offset;
        if (i < 0) i += 6;
        if (i > 5) i -= 6;
        return (Direction)i;
    }

    public static Direction GetOpposite(Direction direction)
    {
        direction += 3;
        if ((int)direction >= 6) direction -= 6;
        return direction;
    }

    public void TurnRight()
    {
        direction++;
        if ((int)direction >= 6) direction = Direction.North;
    }
    public void TurnLeft()
    {
        direction--;
        if (direction < 0) direction = Direction.NorthWest;
    }

    public static Direction TurnLeft(Direction direction)
    {
        direction--;
        if (direction < 0) direction = Direction.NorthWest;
        return direction;
    }
    public static Direction TurnRight(Direction direction)
    {
        direction++;
        if ((int)direction > 5) direction = Direction.North;
        return direction;
    }


    public float Angle
    {
        get
        {
            if (direction == Direction.NorthEast) return 60;
            if (direction == Direction.NorthWest) return -60;
            if (direction == Direction.South) return 180;
            if (direction == Direction.SouthEast) return 120;
            if (direction == Direction.SouthWest) return -120;
            return 0;
        }
    }

    public static float GetAngle(Direction direction)
    {
        if (direction == Direction.NorthEast) return 60;
        if (direction == Direction.NorthWest) return -60;
        if (direction == Direction.South) return 180;
        if (direction == Direction.SouthEast) return 120;
        if (direction == Direction.SouthWest) return -120;
        return 0;
    }

    public Quaternion Rotation { get { return Quaternion.Euler(0, Angle, 0); } }

    public static Quaternion GetRotation(Direction direction)
    {
        return Quaternion.Euler(0, GetAngle(direction), 0);
    }
}
