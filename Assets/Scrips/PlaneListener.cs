using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneListener : MonoBehaviour {
    public Plane Plane;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision collision)
    {
        Plane.OnCollisionEnter(collision);
    }

    public void UseGravity()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
