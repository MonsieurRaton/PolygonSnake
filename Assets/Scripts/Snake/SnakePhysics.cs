using UnityEngine;
using System.Collections;

public class SnakePhysics : EntityPhysics
{
    [Header("Snake Physics")]
    public Snake snake;
    public SnakeVisual head;
    public SnakeVisual body;
    public SnakeVisual tail;

    [Header("Misc")]
    public int selected;

    SnakeVisual[] snakeVisuals = new SnakeVisual[3];

    public GameObject explosionDebug;


    public override void ManualAwake()
    {
        snakeVisuals[0] = head;
        snakeVisuals[1] = body;
        snakeVisuals[2] = tail;
    }

    public override void OnCollisionManual(Collider other)
    {
        if (selected != (int)Snake.PartType.Head) Debug.Log(name + " is not the head -> Ignore collision\n");
        if (selected != (int)Snake.PartType.Head) return;

        // Snake part collision
        if (other.CompareTag(Tags.Snake))
        {
            SnakePhysics otherSnakePhysics = other.GetComponent<Hitbox>().owner.GetComponent<SnakePhysics>();
            otherSnakePhysics.snake.Collision(otherSnakePhysics);
            return;
        }

        // Item
        if (other.CompareTag(Tags.Item))
        {
            Item item = other.GetComponent<Hitbox>().owner.GetComponent<Item>();

            // Hum.. functions or not functions.
            if (item.snakeSizeChange) snake.Grow(item.snakeSizeValue);
            if (item.scoreChange) GameController.instance.gameBoard.score += item.scoreValue;
            if (item.timeChange) GameController.instance.gameBoard.time += item.timeValue;
            if (item.speedChange) { snake.Entity.speed.type = item.speedType; snake.Entity.speedChangeTileDuration = item.speedTileDuration; }

            item.Pick();

            return;
        }

        Debug.Log("Snake collision with " + other.name + " TODO\n");
    }

    public void SetSpeed(float speed)
    {
        for (int i = 0; i < snakeVisuals.Length; i++) snakeVisuals[i].SetSpeed(speed);
    }

    public void ShowVisual(int index)
    {
        selected = index;
        for (int i = 0; i < snakeVisuals.Length; i++)
        {
            if (index == i && !snakeVisuals[i].gameObject.activeSelf) snakeVisuals[i].gameObject.SetActive(true);
            if (index != i && snakeVisuals[i].gameObject.activeSelf) snakeVisuals[i].gameObject.SetActive(false);
        }
    }

    public void ManualDestroy()
    {
        Instantiate(explosionDebug, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
