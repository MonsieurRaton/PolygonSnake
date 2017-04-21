using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Script présent sur la tête du serpent controlable par le joueur. </summary>
public class Snake : MonoBehaviour {

    /// <summary> Liste des élements qui suivent la tête. </summary>
    [SerializeField] private List<GameObject> queue;
    [SerializeField] private SnakeVisuals.Visuels myType;
    [SerializeField] private SnakeVisuals snakeVisuals;
    private SnakeVisual myVisual;

    [SerializeField] private float speed;
    [SerializeField] private float speedRotation;

    void Start () {
        myVisual = snakeVisuals.GetVisualFromType(myType);
        GetComponent<MeshFilter>().mesh = myVisual.meshHead;
        GetComponent<MeshRenderer>().material = myVisual.material;

        for (int i = 0; i<queue.Count; i++) {
            queue[i].GetComponent<MeshRenderer>().material = myVisual.material;
            if (i < queue.Count-1) {
                queue[i].GetComponent<MeshFilter>().mesh = myVisual.meshBody;
            } else {
                queue[i].GetComponent<MeshFilter>().mesh = myVisual.meshQueue;
            }
        }

    }


    private void Update() {
        transform.Translate(0, 0, speed * Time.deltaTime);
        if (Input.GetAxisRaw("Horizontal") != 0) {
            transform.Rotate(Input.GetAxisRaw("Horizontal") * Vector3.up * speedRotation * Time.deltaTime);
        }

        queue[0].transform.LookAt(transform.position - transform.forward * 0.5f);
        queue[0].transform.Translate(0, 0, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, queue[0].transform.position) < 1) {
            queue[0].transform.Translate(0, 0, Vector3.Distance(transform.position, queue[0].transform.position) - 1);
        }
        for (int i = 1; i < queue.Count; i++) {
            queue[i].transform.LookAt(queue[i - 1].transform.position - queue[i - 1].transform.forward*0.5f);
            queue[i].transform.Translate(0, 0, speed * Time.deltaTime);
            if(Vector3.Distance(queue[i].transform.position, queue[i-1].transform.position) < 1){
                queue[i].transform.Translate(0, 0, Vector3.Distance(queue[i].transform.position, queue[i - 1].transform.position)-1);
            }
        }
    }
	
}
