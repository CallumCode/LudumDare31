using UnityEngine;
using System.Collections;

public class ShootingStation : ShootingSprite
{

     // Use this for initialization
    void Start()
    {
        Init();
    }

    override protected void Init()
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

    // Update is called once per frame
    void Update()
    {

        UpdateShoot();
    }

    override protected void UpdateShoot()
    {

       Vector3 targetPos = Input.mousePosition;


       Vector3 dir = targetPos - Camera.main.WorldToScreenPoint(transform.position);
       dir.Normalize();
        turret.transform.up = dir;


        if ((Input.GetButtonDown("Fire1")) && (Time.time > (fireTimer + 1 / fireRate)))
        {
            fireTimer = Time.time;
            Shoot();
        }
    }


}


