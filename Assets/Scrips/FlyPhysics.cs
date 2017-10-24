using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPhysics : MonoBehaviour {
	private Vector3 velocity;
	private Vector3 addedVelocity;
	public float HorizontalMax = 2.5f;
	public float VerticalMax = 1.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();

		if(transform.position.x >= -HorizontalMax && transform.position.x <= HorizontalMax) {
			addedVelocity.x += velocity.x / 40;
		}
		if(transform.position.y >= -VerticalMax && transform.position.y <= VerticalMax) {
			addedVelocity.y += velocity.y / 50;
		}

		Vector3 jitter = new Vector3(Random.Range(-1.0f, 1.0f)/300, Random.Range(-1.0f, 1.0f)/300, 0);
		addedVelocity += jitter;

		addedVelocity.x += (-addedVelocity.x / 4) + (-transform.position.x / 150);
		addedVelocity.y += (-addedVelocity.y / 4) + (-transform.position.y / 150);

		transform.rotation = Quaternion.Euler((transform.position.y/VerticalMax) * 5, 0, (transform.position.x/HorizontalMax) * 15);

		transform.position += addedVelocity;

		velocity = Vector3.zero;
	}

	void GetInput() {
		if(Input.GetButton("Vertical")) {
			velocity.y = Input.GetAxisRaw("Vertical");
		}
		if(Input.GetButton("Horizontal")) {
			velocity.x = Input.GetAxisRaw("Horizontal");
		}
	}
}
