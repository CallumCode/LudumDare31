using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour {

    public GameObject ShootingShipPrefab;
    public GameObject BuilderObject;

    public float basicSpawnRate = 1;
    float basicSpawnTimer = 0;

    public Transform center;

    public float spawnRateGrowth = 0.001f;


    float spawnRadius = 250;
 	// Use this for initialization
	void Start () 
    {
        basicSpawnTimer = Time.time - 1 / basicSpawnRate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SpawnBasicShips();
	}

    void SpawnBasicShips()
    {
        basicSpawnRate += spawnRateGrowth * Time.deltaTime;

        if (Time.time > (basicSpawnTimer + 1 / basicSpawnRate))
        {
            basicSpawnTimer = Time.time;
            Vector2 pos2d = Random.insideUnitCircle * spawnRadius;

            Vector3 pos = center.position + new Vector3(pos2d.x , pos2d.y , 0);

 
            Vector3 dir =  - transform.position;
            dir.Normalize();

            Quaternion orir = Quaternion.LookRotation(Vector3.forward, dir);

             Instantiate(ShootingShipPrefab, pos, orir);

         }

    }

    float GetOffScreen()
    {
        float value = Random.value;
        float pos =  Random.Range(0.8f, 0.9f);

        if (value > 0.5f)
        {
            pos *= -1;
        }

        return pos;
    }
}
