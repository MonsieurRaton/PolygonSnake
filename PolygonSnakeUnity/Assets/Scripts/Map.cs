using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {


    public static Map main;

    public Tile[,] map { get; private set; }

    void Awake () {
        if (main == null) {
            main = this;
        }
	}
	
}
