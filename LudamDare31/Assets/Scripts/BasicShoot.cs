using UnityEngine;
using System.Collections;

public class BasicShoot : MonoBehaviour
{

	public float fireRate = 1;
	float fireRateTimer = 0;
 
	public GameObject projectileObject = null;


	Vector3 clickPoint = Vector3.zero;

 

	// Use this for initialization
	void Start () 
	{
 	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > (fireRateTimer + 1/fireRate))
	    {
			ShootAtMouse();
		}

	}

	

	void ShootAtMouse()
	{
		if (Input.GetButtonDown("Fire1"))
		{		
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray , out hit))			
			{
				clickPoint = new Vector3( hit.point.x , hit.point.y ,  transform.position.z)  ;
				Vector3 dir = clickPoint - transform.position;
				
				dir.Normalize();

				Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);
			

				Instantiate(projectileObject, transform.position ,ori);
 			}
			
		}
	}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
		Gizmos.DrawCube(clickPoint, new Vector3( 0.5f , 0.5f , 0.5f));
		}
}
