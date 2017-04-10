using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {


	public static Vector3 PosistionToGrid(Vector3 v) {
        float y = v.y % 0.5f;
        return new Vector3(Mathf.RoundToInt(v.x), v.y - y, Mathf.RoundToInt(v.z));
	}
}
