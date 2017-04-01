using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
    [Header("Delay before destroy (seconds)")]
    [Range(0, 60)]
    public float delay = 0;

    [Header("Rotation speed (degres/sec")]
    [Range(-180, 180)]
    public float rotationSpeed = 0.75f;


    void Start()
    {
        if (delay > 0) {
            Destroy(gameObject, delay);
        }
        if(rotationSpeed == 0) {
            enabled = false; // N'appelle plus Update()
        }
    }


    private void Update() {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
