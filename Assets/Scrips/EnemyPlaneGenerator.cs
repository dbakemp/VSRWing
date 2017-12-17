using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneGenerator : MonoBehaviour {
    public GameObject EnemyPlanePrefab;
    public GameObject ClippingPlane;
    public GameObject ShootingVisor;
    float spawnDelay = 1;
    float spawnTick = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(spawnTick >= spawnDelay)
        {
            GameObject plane = Instantiate(EnemyPlanePrefab);
            spawnTick = 0;
            spawnDelay = Random.Range(1f, 3f);
            
        }

        spawnTick += Time.deltaTime;
	}
}
