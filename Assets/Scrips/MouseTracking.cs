using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracking : MonoBehaviour {
	void Start () {
		
	}
	
	void Update () {
        var pos = Input.mousePosition;
        pos.z = 24;
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = pos;
    }
}
