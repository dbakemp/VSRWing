using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    public EnemyPlaneListener EnemyPlaneListener;
    public ParticleSystem ParticleSystem;
    public GameObject HealthBar;
    public BulletEmitter TurretA;
    private Vector3 velocity;
    private Vector3 addedVelocity;
    private bool isDead = false;
    private int health = 100;
    private Vector3 origin;
    private Vector3 destination = new Vector3(0, 0, -5);
    private Vector3 originOffset;
    private Vector3 destinationOffset;
    public float MoveSpeed = 1.0f;
    public float MoveStep = 0.0f;
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
        TurretA.IsShooting = true;
        origin = gameObject.transform.localPosition;
        originOffset = origin + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        destinationOffset = destination + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
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

            if(MoveStep <= 1.0f)
            {
                MoveStep += 0.15f * Time.deltaTime;
                transform.position = BezierGenerator.BezierPathCalculation(origin, originOffset, destinationOffset, destination, MoveStep);
            }

            if(Vector3.Distance(transform.position, destination) <= 5f)
            {
                remove();
            } 
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "BulletGreen(Clone)")
        {
            Health -= 25;
            Destroy(collision.gameObject);
            TurretA.IsShooting = false;
        }
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

    private void remove()
    {
        DestroyObject(gameObject);
    }
}
