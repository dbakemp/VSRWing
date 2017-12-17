using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {
    public GameObject ClippingPlane;
    public GameObject ShootingVisor;
    public Transform BulletPrefab;
    public float ShootDelay = 0.15f;
    private float timeLastBullet = 0;
    private List<Bullet> bullets = new List<Bullet>();
    public bool IsShooting = false;

	void Start () {
		if(ClippingPlane == null)
        {
            ClippingPlane = GameObject.Find("Plane");
        }

        if (ShootingVisor == null)
        {
            ShootingVisor = GameObject.Find("PlayerPlane");
        }
    }
	
	void Update () {
        if ((IsShooting || Input.GetMouseButton(0)) && !GameObject.Find("Controller").GetComponent<GameController>().GameOverBool && ShootDelay < timeLastBullet)
        {
            bullets.Add(new Bullet(transform.position, ShootingVisor.transform.position, ClippingPlane.transform.position, BulletPrefab));
            timeLastBullet = 0;
        }

        foreach (Bullet bullet in bullets)
        {
            bullet.Update();
        }

        if(bullets.Count > 50)
        {
            int toRemove = bullets.Count - 50;
            for(int i = 0; i < toRemove; i++)
            {
                bullets[0].Destroy();
                bullets[0] = null;
                bullets.RemoveAt(0);
            }
        }
        timeLastBullet += Time.deltaTime;
    }
}

public class Bullet
{
    GameObject gameObject;
    Vector3 originPosition;
    Vector3 visorPosition;
    Vector3 endPosition;

    float startTime;
    float speed = 60.0f;
    float journeyLength;

    public Bullet(Vector3 origin, Vector3 visor, Vector3 end, Transform BulletPrefab)
    {
        this.originPosition = origin;
        this.visorPosition = visor;
        Vector3 directionNormal = Vector3.Normalize(visorPosition - originPosition);
        endPosition = directionNormal * (end.z - originPosition.z);

        gameObject = Object.Instantiate(BulletPrefab).gameObject;
        
        gameObject.transform.position = origin;

        startTime = Time.time;
        journeyLength = Vector3.Distance(originPosition, endPosition);
        
    }

    public void Update()
    {
        if(gameObject!= null)
        {
            var distCovered = (Time.time - startTime) * speed;
            var fracJourney = distCovered / journeyLength;

            gameObject.transform.position = Vector3.Lerp(originPosition, endPosition, fracJourney);
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}