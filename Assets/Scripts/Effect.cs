using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
    [Header("Delay (seconds)")]
    public bool delay;
    [Range(0, 60)]
    public float delayValue = 1;

    [Header("Rotation speed")]
    public bool rotate;
    [Range(-2, 2)]
    public float rotationSpeed = 0.75f;


    void Start()
    {
        if (delay) StartCoroutine(Play());
        if (rotate) StartCoroutine(Rotate());
    }


    IEnumerator Play()
    {
        yield return new WaitForSeconds(delayValue);

        Destroy(gameObject);
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, rotationSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
