using UnityEngine;
using System.Collections;

public class EntityPhysics : MonoBehaviour
{
    Rigidbody rb;

    public GameObject hitbox;
    public GameObject visual;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ManualAwake();
    }

    public virtual void ManualAwake() { }


    public Vector3 Position { get { return transform.position; } }

    public void Move(HexaGridPosition current, HexaGridPosition next, float speedStepRatio)
    {
        rb.MovePosition(current.WorldPosition + ((next.WorldPosition - current.WorldPosition) * speedStepRatio));
    }

    public Quaternion Rotation { get { return transform.rotation; } }

    public void Rotate(Direction direction)
    {
        transform.rotation = HexaDirection.GetRotation(direction);
    }
    
    public void SetHitboxActive(bool state) { hitbox.SetActive(state); }


    void OnTriggerEnter(Collider other)
    {
        OnCollisionManual(other);
    }

    public virtual void OnCollisionManual(Collider other)
    {
        Debug.Log("EntityPhysics collision call from " + name + ", with " + other.name + "\n");
    }
}
