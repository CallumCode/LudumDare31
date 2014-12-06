using UnityEngine;
using System.Collections;

public class BasicShip : MonoBehaviour
{

    public Transform target;

    float maxRadDelt = .1f;
    float maxMag = 1;

    float speed = 2;

    float range = 1.25f;

    float startDist = 1;
	// Use this for initialization
	void Start ()
    {
        startDist = Vector3.Distance(target.position, transform.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
	}


    void Movement()
    {
        Vector3 fly = Vector3.Normalize(target.position - transform.position);
        Vector3 orbit = Vector3.Cross(fly, Vector3.forward);
        float t = Vector3.Distance(target.position, transform.position) / (startDist );
        t = range - Mathf.Clamp01(t);
        t = Mathf.Clamp01(t);
        Vector3 dir = Vector3.Lerp(fly , orbit, t );
        dir.Normalize();
        transform.up = dir;
        transform.Translate(Vector3.up * Time.deltaTime * speed);

    }
 
}
