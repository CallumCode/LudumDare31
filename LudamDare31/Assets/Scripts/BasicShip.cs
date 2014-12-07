using UnityEngine;
using System.Collections;

public class BasicShip : MonoBehaviour
{

     public GameObject projectileObject = null;
 
    public GameObject BarPrefab;
    GameObject hpBarObject;

    BarScript hpBarScript;

    float speed = 2;

   public  float rangeFraction = 0.2f;


    float startDist = 1;

    public float fireRate = 0.5f;
    float fireTimer = 0;

    public float crashDamage = 10;

    const float maxHealth = 100;
    public float health = maxHealth;

    Vector3 centerPos = Vector3.zero;

    Builder builderScript;

    
	// Use this for initialization
	void Start ()
    {
 	}

    public virtual void Init(Builder builder)
    {
        builderScript = builder;

        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(11, 11);

       hpBarObject = Instantiate(BarPrefab, transform.position, Quaternion.identity) as GameObject;
       hpBarScript = hpBarObject.GetComponentInChildren<BarScript>();
       hpBarScript.objectToFollow = transform;
      
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Normalize(centerPos - transform.position));
        Vector3 target = Vector3.zero;
        if (hit.collider != null && renderer.isVisible == true) 
        {
           target = new Vector3(hit.point.x, hit.point.y, 0);
        } 
           
       startDist = Vector3.Distance(target, transform.position);

    }
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        Shooting();

        if (health <= 0)
        {
            DestroySelf();
        }
	}

    void Shooting()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Normalize(centerPos - transform.position));
        if ((Time.time > (fireTimer + 1/fireRate ) ) && (hit.collider != null) && ( hit.collider.CompareTag("Station")) )
        {
            Vector3 target = new Vector3(hit.point.x, hit.point.y, 0);

            fireTimer = Time.time;

            Vector3 dir = target - transform.position;
            dir.Normalize();
            Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);

            Instantiate(projectileObject, transform.position, ori);
        
        }
    }

    void Movement()
    {



        // check inwards.
      //  RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector3.Normalize(centerPos - transform.position));
        
     //   if ((hit.collider != null))//&& hit.collider.CompareTag("Station"))
      //  {


            //   Debug.Log(hit.collider.tag);
            Vector3 target = builderScript.GetAvgPos();

            Vector3 fly = Vector3.Normalize(target - transform.position);
            Vector3 orbit = Vector3.Cross(fly, Vector3.forward);
            float t = Vector3.Distance(target, transform.position) / (startDist);
            t = (1 + rangeFraction) - Mathf.Clamp01(t);
            t = Mathf.Clamp01(t);
            Vector3 dir = Vector3.Lerp(fly, orbit, t);
            dir.Normalize();
            transform.up = dir;

        //}
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        DoCollision(coll);
    }

    protected virtual void DoCollision(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("StationProjectile"))
        {
            BasicProjectile basicProjectile = coll.gameObject.GetComponent<BasicProjectile>();
            TakeDamage(basicProjectile.damage);
        }
    }
 

   public void TakeDamage(float dam)
    {
        health -= dam;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (hpBarScript != null) hpBarScript.SetValue(health / maxHealth);
    }

    protected virtual void DestroySelf()
    {
        if(hpBarObject != null)   Destroy(hpBarObject);
        Destroy(gameObject);
    }

}
