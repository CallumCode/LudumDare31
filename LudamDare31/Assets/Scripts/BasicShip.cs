using UnityEngine;
using System.Collections;

public class BasicShip : MonoBehaviour
{

    public Transform target;
    public GameObject projectileObject = null;
 
    public GameObject BarPrefab;
    GameObject hpBarObject;

    BarScript hpBarScript;

    float speed = 2;

    float range = 1.5f;

    float startDist = 1;

    public float fireRate = 0.5f;
    float fireTimer = 0;

    public float crashDamage = 10;

    const float maxHealth = 100;
    public float health = maxHealth;

	// Use this for initialization
	void Start ()
    {
        Init();
	}

    protected virtual void Init()
    {
        Physics2D.IgnoreLayerCollision(9, 11);

       hpBarObject = Instantiate(BarPrefab, transform.position, transform.rotation) as GameObject;
       hpBarScript = hpBarObject.GetComponentInChildren<BarScript>();
       hpBarScript.objectToFollow = transform;

       startDist = Vector3.Distance(target.position, transform.position);

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
        if (Time.time > (fireTimer + fireRate / 1) && (target != null))
        {
            fireTimer = Time.time;

            Vector3 dir = target.position - transform.position;
            dir.Normalize();
            Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);

            Instantiate(projectileObject, transform.position, ori);
        
        }
    }

    void Movement()
    {
        if (target != null)
        {
            Vector3 fly = Vector3.Normalize(target.position - transform.position);
            Vector3 orbit = Vector3.Cross(fly, Vector3.forward);
            float t = Vector3.Distance(target.position, transform.position) / (startDist);
            t = range - Mathf.Clamp01(t);
            t = Mathf.Clamp01(t);
            Vector3 dir = Vector3.Lerp(fly, orbit, t);
            dir.Normalize();
            transform.up = dir;
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        DoCollision(coll);
    }

    protected virtual void DoCollision(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Projectile"))
        {
            BasicProjectile basicProjectile = coll.gameObject.GetComponent<BasicProjectile>();
            TakeDamage(basicProjectile.damage);
        }

    }

 

    void TakeDamage(float dam)
    {
        health -= dam;
        health = Mathf.Clamp(health, 0, maxHealth);

        hpBarScript.SetValue(health / maxHealth);
    }

    protected virtual void DestroySelf()
    {
        Destroy(hpBarObject);
        Destroy(gameObject);
    }
}
