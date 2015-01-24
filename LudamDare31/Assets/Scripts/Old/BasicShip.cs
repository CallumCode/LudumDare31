using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicShip : SpriteBase
{

    public GameObject projectileObject = null; 
 
    float speed = 2;

    public float rangeFraction = 0.2f;


    float startDist = 1;

    public float fireRate = 0.5f;
    float fireTimer = 0;

    public float crashDamage = 10;
 
    Vector3 centerPos = Vector3.zero;

  
    AudioSource shootNoise;
    AudioSource explosionNoise;

     public GameObject laserSpawn;
    public GameObject turret;


 
    // Use this for initialization
    void Start()
    {
    }

    public override void Init()
    {
      //  base.Init();
 
        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(11, 11);
 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Normalize(centerPos - transform.position));
        Vector3 target = Vector3.zero;
        if (hit.collider != null && renderer.isVisible == true)
        {
            target = new Vector3(hit.point.x, hit.point.y, 0);
        }

        startDist = Vector3.Distance(target, transform.position);

        AudioSource[] sources = GetComponents<AudioSource>();

        shootNoise = sources[0];
        explosionNoise = sources[1];

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();

        if (health <= 0)
        {
            float time = shootNoise.clip.length;
            explosionNoise.Play();

            Invoke("DestroySelf", time);
        }
    }

    void Shooting()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Normalize(centerPos - transform.position));
        if ((hit.collider != null) && (hit.collider.CompareTag("Station")))
        {
            Vector3 target = new Vector3(hit.point.x, hit.point.y, 0);


            Vector3 dir = target - transform.position;
            dir.Normalize();
          turret.transform.up = dir;

            if (Time.time > (fireTimer + 1 / fireRate) && transform.renderer.isVisible == true)
            {


                fireTimer = Time.time;

                Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);

                Instantiate(projectileObject, laserSpawn.transform.position, ori);
                shootNoise.pitch = Random.Range(0.75f, 1.25f);
                shootNoise.Play();

            }
        }
    }

    void Movement()
    {



        // check inwards.
        //  RaycastHit2D hit = Physics2D.Raycast(transform.position , Vector3.Normalize(centerPos - transform.position));

        //   if ((hit.collider != null))//&& hit.collider.CompareTag("Station"))
        //  {


        //   Debug.Log(hit.collider.tag);
        Vector3 target = Vector3.zero; //builderScript.GetAvgPos();

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



    protected override void DoCollision(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("StationProjectile"))
        {
            BasicProjectile basicProjectile = coll.gameObject.GetComponent<BasicProjectile>();
            TakeDamage(basicProjectile.damage);
        }
    }

 

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

}
