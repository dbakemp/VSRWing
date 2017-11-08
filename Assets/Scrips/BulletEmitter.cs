using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {
    public GameObject ClippingPlane;
    public GameObject ShootingVisor;
    private List<Bullet> bullets = new List<Bullet>();

	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButton(0))
        {
            bullets.Add(new Bullet(transform.position, ShootingVisor.transform.position, ClippingPlane.transform.position));
        }

        foreach (Bullet bullet in bullets)
        {
            bullet.Update();
        }

        if(bullets.Count > 100)
        {
            int toRemove = bullets.Count - 100;
            for(int i = 0; i < toRemove; i++)
            {
                bullets[0].Destroy();
                bullets[0] = null;
                bullets.RemoveAt(0);
            }
        } 
	}
}

public class Bullet
{
    GameObject gameObject;
    Vector3 originPosition;
    Vector3 visorPosition;
    Vector3 endPosition;

    float startTime;
    float speed = 30.0f;
    float journeyLength;

    public Bullet(Vector3 origin, Vector3 visor, Vector3 end)
    {
        this.originPosition = origin;
        this.visorPosition = visor;
        Vector3 directionNormal = Vector3.Normalize(visorPosition - originPosition);
        endPosition = directionNormal * (end.z - originPosition.z);

        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.transform.position = origin;
        gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        gameObject.AddComponent<TrailRenderer>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        startTime = Time.time;
        journeyLength = Vector3.Distance(originPosition, endPosition);


        TrailRenderer trailRenderer = GameObject.Find("Prefabs").GetComponent<TrailRenderer>();
        if(UnityEditorInternal.ComponentUtility.CopyComponent(trailRenderer))
        {
            if (UnityEditorInternal.ComponentUtility.PasteComponentValues(gameObject.GetComponent<TrailRenderer>()))
            {
                gameObject.GetComponent<TrailRenderer>().enabled = true;
            }
        }
    }

    public void Update()
    {
        var distCovered = (Time.time - startTime) * speed;
        var fracJourney = distCovered / journeyLength;
        
        gameObject.transform.position = Vector3.Lerp(originPosition, endPosition, fracJourney);
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}