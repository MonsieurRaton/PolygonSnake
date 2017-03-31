using UnityEngine;
using System.Collections;

public class SnakeDatabase : MonoBehaviour
{
    public SnakePhysics[] snakes;


    public SnakePhysics NewSnake(int snakeIndex, Vector3 position, Quaternion rotation)
    {
        return (SnakePhysics)Instantiate(snakes[snakeIndex], position, rotation);
    }
}
