using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallGameScene()
    {
            Application.LoadLevel("Gameplay");
    }
}
