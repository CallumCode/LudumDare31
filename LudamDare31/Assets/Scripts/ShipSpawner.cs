using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour {

    public GameObject BasicShootPrefab;
    public GameObject BuilderObject;

    public float basicSpawnRate = 1;
    float basicSpawnTimer = 0;

    public Transform center;

    public float spawnRateGrowth = 0.001f;

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
        basicSpawnRate += spawnRateGrowth * Time.deltaTime;

        if (Time.time > (basicSpawnTimer + 1 / basicSpawnRate))
        {
            basicSpawnTimer = Time.time;
            Vector3 pos = new Vector3(GetOffScreen(), GetOffScreen() , -Camera.main.gameObject.transform.position.z);
            pos = Camera.main.ViewportToWorldPoint(pos);

            Vector3 dir =  - transform.position;
            dir.Normalize();

            Quaternion orir = Quaternion.LookRotation(Vector3.forward, dir);

            GameObject ship =  Instantiate(BasicShootPrefab, pos, orir) as GameObject;

            ship.GetComponent<BasicShip>().Init(BuilderObject.GetComponent<Builder>());
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
