using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {


    public static Map main;

    public Tile[,] map;

    void Awake () {
        if (main == null) {
            main = this;
        }
	}
	
}
