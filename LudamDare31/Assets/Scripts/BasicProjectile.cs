using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour
{

    public float damage = 50;
    public float speed = 10;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame	
	void Update () 
	{
		Movement();	
	}

	void Movement()
	{
		transform.Translate( Vector3.up * Time.deltaTime * speed );
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
