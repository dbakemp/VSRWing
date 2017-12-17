using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneListener : MonoBehaviour {
    public EnemyPlane EnemyPlane;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyPlane.OnCollisionEnter(collision);
    }

    public void UseGravity()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
