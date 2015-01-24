using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour
{

    public float damage = 50;
    public float force = 10;
	// Use this for initialization
	void Start () 
	{
        rigidbody2D.AddForce(transform.up * force);

	}
	
	// Update is called once per frame	
	void Update () 
	{
 	}
    	 

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
