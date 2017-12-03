using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    public EnemyPlaneListener EnemyPlaneListener;
    public ParticleSystem ParticleSystem;
    public GameObject HealthBar;
    private Vector3 velocity;
    private Vector3 addedVelocity;
    private bool isDead = false;
    private int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            HealthBar.GetComponent<HealthBar>().SetPercentage(health);
            if(health <= 0)
            {
                explode();
                isDead = true;
            }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!isDead)
        {
            /*Vector3 jitter = new Vector3(Random.Range(-1.0f, 1.0f) / 300, Random.Range(-1.0f, 1.0f) / 300, 0);
            addedVelocity += jitter;

            addedVelocity.x += (-addedVelocity.x / 4) + (-transform.position.x / 150);
            addedVelocity.y += (-addedVelocity.y / 4) + (-transform.position.y / 150);
            transform.position += addedVelocity;

            velocity = Vector3.zero;*/

        } 
    }

    public void OnCollisionEnter(Collision collision)
    {
        Health -= 5;
    }

    private void explode()
    {
        if(!isDead)
        {
            EnemyPlaneListener.UseGravity();
            ParticleSystem.Play();
            DestroyObject(gameObject, ParticleSystem.main.duration);
        }
    }
}
