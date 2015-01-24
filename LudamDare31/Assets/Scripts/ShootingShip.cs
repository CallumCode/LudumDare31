using UnityEngine;
using System.Collections;

public class ShootingShip : ShootingSprite
{

    GameObject centerObject;

    float turnForce = 10;
    public float moveForce = 100;

    float startDist = 0;

    float turnScaler = 0;
    float moveScaler = 0;
    // Use this for initialization
	void Start ()
    {
        Init();
	}
	
 
    override public void Init()
    {
        base.Init();
        GetSounds();

        centerObject = GameObject.Find("StartTransform");

        if (centerObject)
        {
            startDist = Vector3.Distance(transform.position, centerObject.transform.position);
        }
        else
        {
            Debug.Log("centerobject is null");
            Debug.Break();
        }
    }   

    void GetSounds()
    {
        AudioSource[] AudioSources = GetComponents<AudioSource>();

        deathSound = AudioSources[0];
        shootSound = AudioSources[1];

    }

    // Update is called once per frame
    void Update()
    {
        UpdateShoot();
        Movement(); 
    }

    void Movement()
    {
        if (centerObject)
        {
            Vector3 dir = (centerObject.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.up, dir);
              turnScaler = dot / 90;
             
            rigidbody2D.AddForceAtPosition(transform.right * turnScaler * turnForce * Time.deltaTime , transform.position + transform.up + transform.right);

            float dist = Vector3.Distance(transform.position, centerObject.transform.position);

            moveScaler = 1; // startDist / dist;

 
            rigidbody2D.AddForce(transform.up *  Time.deltaTime * moveForce * moveScaler);
        }

    }
}
