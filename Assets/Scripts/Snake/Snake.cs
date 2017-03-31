using UnityEngine;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    [Header("Database")]
    public SnakeDatabase snakeDatabase;

    [Header("Snake")]
    [Range(0, 0)]
    public int snakeIndex = 0;
    [Range(2, 30)]
    public int maxParts = 20;
    [Range(180, 300)]
    public int blockCycleLimit = 180;
    
    public HexaEntity Entity { get; set; }

    [Header("Parts (Visual Debug)")]
    public int block;
    public int growth;
    public List<Part> parts;
    public Part Latest { get { return parts[parts.Count - 1]; } }

    public enum PartType
    {
        Head,
        Body,
        Tail
    }


    public void Init(HexaEntity entity)
    {
        Entity = entity;

        StartingPosition startingPosition = GameController.instance.GetStartingPosition(0);
        startingPosition.MoveNextSpawnPosition();

        entity.SetGridPosition(startingPosition.GetSpawnPosition().hexaGridPosition);
        entity.SetDirection(startingPosition.GetSpawnPosition().direction);
        
        NewSnakePhysics(PartType.Head, entity.positionCurrent, entity.hexaDirection.direction);
        entity.entityPhysics = Latest.snakePhysics;
        Latest.snakePhysics.SetHitboxActive(true);
        Latest.Init(entity);

        HexaGridPosition gridPosition = new HexaGridPosition(entity.positionCurrent);

        gridPosition.Move(HexaDirection.GetOpposite(entity.hexaDirection.direction));//body+
        NewSnakePhysics(PartType.Body, gridPosition, Latest.direction);
        Latest.InitSpecial(parts[parts.Count - 2]);

        gridPosition.Move(HexaDirection.GetOpposite(entity.hexaDirection.direction));//tail
        NewSnakePhysics(PartType.Body, gridPosition, Latest.direction);
        Latest.InitSpecial(parts[parts.Count - 2]);
    }


    public GameObject HeadGameObject { get { return parts[0].snakePhysics.gameObject; } }

    public void NewSnakePhysics(PartType partType, HexaGridPosition position, Direction direction)
    {
        if (partType == PartType.Body)
        {
            if (parts.Count >= 2) Latest.snakePhysics.ShowVisual((int)PartType.Body);
            partType = PartType.Tail;
        }

        parts.Add(new Part());

        Latest.CreateSnakePhysics(snakeDatabase, snakeIndex, partType, position, direction);
        Latest.snakePhysics.snake = this;
    }


    public void UpdateChain()
    {
        if (block > 0)
        {
            block++;
            if (block >= blockCycleLimit)
            {
                Debug.Log("Block time limit\n");
                Entity.Destroy();
                return;
            }
        }

        parts[0].snakePhysics.SetSpeed(Entity.speed.NormalizedRatio);

        for (int i = 1; i < parts.Count; i++)
        {
            parts[i].Update(Entity.speed);
        }
    }

    public void DisplaceChain()
    {
        if (growth > 0)
        {
            Grow();
            growth--;
            if (growth > 0)
            {
                if (parts.Count >= maxParts) { growth = 0; Debug.Log("LIMIT \n"); } else { Debug.Log(growth + " more left \n"); }
            }
        }

        parts[0].Displace(Entity);

        for (int i = 1; i < parts.Count; i++) { parts[i].Displace(parts[i - 1]); }
    }

    public void Grow(int size) { growth += size; }
    void Grow()
    {
        NewSnakePhysics(PartType.Body, Latest.position, Latest.direction);
        Latest.Init(parts[parts.Count - 2]);
    }

    public void ShrinkAt(int index)
    {
        if (index <= 2)
        {
            Debug.Log("Fatal damage\n");
            Entity.Destroy();
            return;
        }

        growth = 0;// Damage negates growth

        for (int i = parts.Count - 1; i >= index; i--)
        {
            parts[i].Destroy();
            parts.RemoveAt(i);
        }

        if (index - 1 > 0) parts[index - 1].snakePhysics.ShowVisual((int)PartType.Tail);

        Debug.Log("Chain destroyed at " + index + "\n");
    }

    public void Block()
    {
        Debug.Log("Block damage\n");
        DamageToHalf();
        block++;
    }

    public void Damage(int size)
    {
        ShrinkAt(parts.Count - size);
    }

    public void DamageToHalf()
    {
        ShrinkAt(Mathf.FloorToInt(parts.Count / 2f) + 1);
    }

    public void Collision(SnakePhysics snakePhysics)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (snakePhysics == parts[i].snakePhysics)
            {
                ShrinkAt(i);
                break;
            }
        }
    }

    public void Destroy()
    {
        for (int i = 0; i < parts.Count; i++) parts[i].Destroy();
        Destroy(gameObject);
    }



    [System.Serializable]
    public class Part
    {
        public SnakePhysics snakePhysics = null;
        public HexaGridPosition position = new HexaGridPosition();
        public HexaGridPosition destination = new HexaGridPosition();
        public Direction direction = Direction.North;
        public Direction directionPrevious = Direction.North;
        public bool waitForNextCycle;
        

        public void Init(HexaEntity entity)
        {
            position.Copy(entity.positionCurrent);
            destination.Copy(entity.positionNext);
            direction = entity.hexaDirection.direction;
            directionPrevious = direction;
            waitForNextCycle = false;
        }

        public void Init(Part other)
        {
            position.Copy(other.position);
            destination.Copy(other.destination);
            direction = other.direction;
            directionPrevious = other.directionPrevious;
            waitForNextCycle = true;
        }

        //TODO: Init(s) à simplifier >_<
        public void InitSpecial(Part other)
        {
            destination.Copy(other.position);
            directionPrevious = direction;
            waitForNextCycle = true;
        }

        public void CreateSnakePhysics(SnakeDatabase snakeDatabase, int snakeIndex, PartType partType, HexaGridPosition position, Direction direction)
        {
            this.position.Copy(position);
            this.direction = direction;
            snakePhysics = snakeDatabase.NewSnake(snakeIndex, position.WorldPosition, HexaDirection.GetRotation(direction));
            snakePhysics.ShowVisual((int)partType);
        }

        public void Displace(HexaEntity entity)
        {
            position.Copy(entity.positionCurrent);
            destination.Copy(entity.positionNext);
            directionPrevious = entity.directionPrevious;
            direction = entity.hexaDirection.direction;
        }
        public void Displace(Part front)
        {
            if (waitForNextCycle)
            {
                waitForNextCycle = false;
                return;
            }

            if (!snakePhysics.hitbox.activeSelf) snakePhysics.SetHitboxActive(true);

            position.Copy(destination);
            destination.Copy(front.position);
            directionPrevious = direction;
            direction = front.directionPrevious;
        }

        public void Update(Speed speed)
        {
            snakePhysics.SetSpeed(speed.NormalizedRatio);

            if (waitForNextCycle) return;

            snakePhysics.Move(position, destination, speed.StepSpeedRatio);
            snakePhysics.Rotate(direction);
        }

        public void Destroy()
        {
            snakePhysics.ManualDestroy();
        }
    }
}
