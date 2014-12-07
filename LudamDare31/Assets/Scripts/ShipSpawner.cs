using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour {

    public GameObject BasicShootPrefab;

    public float basicSpawnRate = 1;
    float basicSpawnTimer = 0;

    public Transform center;

    float dist = 2;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        SpawnBasicShips();
	}

    void SpawnBasicShips()
    {
        if (Time.time > (basicSpawnTimer + 1 / basicSpawnRate))
        {
            basicSpawnTimer = Time.time;
            Vector3 pos = Vector3.zero;
            pos.x = Random.value + 0.25f;
            pos.x = Mathf.Clamp01(pos.x);
            if (Random.value > 0.5F) pos.x *= -1;
            pos.x *= dist;

            pos.y = Random.value;
            pos.y = Mathf.Clamp01(pos.y);
            if (Random.value > 0.5F) pos.y *= -1;
            pos.y *= dist;

            Vector3 dir = pos - transform.position;
            dir.Normalize();

            Quaternion orir = Quaternion.LookRotation(Vector3.forward, dir);

            GameObject ship =  Instantiate(BasicShootPrefab, pos, orir) as GameObject;

            ship.GetComponent<BasicShip>().Init();
        }

    }
}
