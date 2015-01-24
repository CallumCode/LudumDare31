using UnityEngine;
using System.Collections;

public class ShootingSprite : SpriteBase
{

   public GameObject projectlePrefab;
   public GameObject projectleSpawn;
   public GameObject turret;

   public Transform target = null;

   public float fireRate = 1;
   protected float fireTimer = 0;

   protected AudioSource shootSound;
   float shootMinPitch = 0.9f;
   float shootMaxPitch = 1.1f;

    // Use this for initialization
	void Start () 
    {
        Init();
  	}

    // Update is called once per frame
    void Update()
    {
        UpdateShoot();
	}

  public  override void Init()
    {
      base.Init();
      GetSounds();
   }

   void GetSounds()
   { 
       AudioSource[] AudioSources = GetComponents<AudioSource>();

       deathSound = AudioSources[0];
       shootSound = AudioSources[1];
   
   }

   protected void Shoot()
    {
        shootSound.pitch = Random.Range(shootMinPitch, shootMaxPitch); 
        shootSound.Play();     
        Instantiate(projectlePrefab, projectleSpawn.transform.position, projectleSpawn.transform.rotation);    
    }

    protected virtual void UpdateShoot()
    {
        if (target != null)
        {

            Vector3 dir = target.position - transform.position;
            dir.z = 0;

            turret.transform.up = dir;

            if (Time.time > (fireTimer + 1 / fireRate))
            {
                fireTimer = Time.time;
                Shoot();
            }
        }
    }

	

}

