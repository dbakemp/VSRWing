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

            float MaxSpawnDelay = 5f - (GameObject.Find("Controller").GetComponent<GameController>().Level/7);
            if(MaxSpawnDelay < 1f)
            {
                MaxSpawnDelay = 1f;
            }

            spawnDelay = Random.Range(1f, MaxSpawnDelay);
            
        }

        spawnTick += Time.deltaTime;
	}
}
