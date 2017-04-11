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
	
}
