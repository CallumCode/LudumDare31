﻿using UnityEngine;
using System.Collections;

public class BasicShoot : MonoBehaviour
{
    public GameObject projectileObject = null;
    public GameObject BarPrefab;

    GameObject hpBarObject;
    BarScript hpBarScript;

    public float fireRate = 1;
    float fireRateTimer = 0;

    Vector3 clickPoint = Vector2.zero;

    const float maxHealth = 100;
    public float health = maxHealth;

    public enum ModuleStateType { shop, preview, built };
    public ModuleStateType moduleState = ModuleStateType.preview;


    public Sprite shopSprite;
    public Sprite previewSprite;
    public Sprite buildSprite;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;

    Builder builderScript;
    bool buildOK = false;
    bool stationTouching = false;

    public GameObject buildIndicatorObject;
    SpriteRenderer buildIndicatorRenderer;

    public Sprite buildOKSprite;
    public Sprite buildBadSprite;

    public int listIndex = -1;

    AudioSource shootNoise;
    AudioSource explosionNoise;

    public GameObject turret;

    public GameObject laserSpawn;
    public GameObject hpSpawn;

    // Use this for initialization
    void Start()
    {

    }
    
    public virtual void Init(Builder builder)
    {
        Physics2D.IgnoreLayerCollision(8, 10);

        builderScript = builder;
 
        hpBarObject = Instantiate(BarPrefab, hpSpawn.transform.position, transform.rotation) as GameObject;
        hpBarScript = hpBarObject.GetComponentInChildren<BarScript>();
        hpBarScript.objectToFollow = hpSpawn.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        buildIndicatorRenderer = buildIndicatorObject.GetComponent<SpriteRenderer>();

        AudioSource[] sources = GetComponents<AudioSource>();

        shootNoise = sources[0];
        explosionNoise = sources[1];

    }
    // Update is called once per frame
    void Update()
    {
        switch (moduleState)
        {
            case ModuleStateType.preview:
                {
                    PreviewUpdate();
                }
                break;
            case ModuleStateType.built:
                {
                    BuiltUpdate();
                }
                break;
            case ModuleStateType.shop:
                {
                    ShopUpdate();
                }
                break;
        }


    }

    void PreviewUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }

        if (buildOK == true && stationTouching == false)
        {

            if (Input.GetMouseButtonDown(1))
            {
                builderScript.RestockBasicShooter();
                ChaingeState(ModuleStateType.built);
            }

            buildIndicatorRenderer.sprite = buildOKSprite;
        }
        else
        {
            buildIndicatorRenderer.sprite  = buildBadSprite;
        }

        buildOK = false;
        stationTouching = false;
    }
    void ShopUpdate()
    {
        if(builderScript.money > builderScript.basiShootCost)
        {
            buildIndicatorRenderer.sprite = buildOKSprite;
        }
        else
        {
            buildIndicatorRenderer.sprite = buildBadSprite;
        }
    }

    void BuiltUpdate()
    {
        ShootAtMouse();

        if (health <= 0)
        {
            float time = shootNoise.clip.length;
            explosionNoise.Play();

            Invoke("DestroySelf", time);
         }
    }

    void ShootAtMouse()
    {
        clickPoint = Input.mousePosition;


        Vector3 dir = clickPoint - Camera.main.WorldToScreenPoint(transform.position);
        dir.Normalize();

        turret.transform.up = dir;

        if (Input.GetButtonDown("Fire1") && (Time.time > (fireRateTimer + 1 / fireRate)))
        {
            fireRateTimer = Time.time;
            shootNoise.pitch = Random.Range(0.9f, 1.1f);

            shootNoise.Play();


        
            Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);
            Instantiate(projectileObject, laserSpawn.transform.position, ori);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        DoCollision(coll);
    }

    protected virtual void DoCollision(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("ShipProjectile"))
        {
            BasicProjectile basicProjectile = coll.gameObject.GetComponent<BasicProjectile>();
            TakeDamage(basicProjectile.damage);
            Destroy(coll.gameObject);
        }


        if (coll.gameObject.CompareTag("Ship"))
        {
            BasicShip basicShip = coll.gameObject.GetComponent<BasicShip>();
            TakeDamage(basicShip.crashDamage);
            basicShip.TakeDamage(100);
        }
    }

    void TakeDamage(float dam)
    {
        health -= dam;
        health = Mathf.Clamp(health, 0, maxHealth);

        hpBarScript.SetValue(health / maxHealth);
    }


    void DestroySelf()
    {

        builderScript.ModuleLost(listIndex);
        Destroy(hpBarObject);
        Destroy(gameObject);
    }

    public void ChaingeState(ModuleStateType newState)
    {
        switch (newState)
        {
            case ModuleStateType.preview:
                {
                    Color color = renderer.material.color;
                    color.a = 0.5f;
                    renderer.material.color = color;
                    spriteRenderer.sprite = previewSprite;
                    buildIndicatorObject.SetActive(true);

                }
                break;
            case ModuleStateType.built:
                {
                    Color color = renderer.material.color;
                    color.a = 1;
                    renderer.material.color = color;
                    hpBarObject.SetActive(true);
                    spriteRenderer.sprite = buildSprite;
                    boxCollider2D.isTrigger = false;

                    buildIndicatorObject.SetActive(false);

                    listIndex = builderScript.modulesList.Count;
                    builderScript.modulesList.Add(gameObject);
                }
                break;
            case ModuleStateType.shop:
                {
                    hpBarObject.SetActive(false);
                    spriteRenderer.sprite = shopSprite;
                    boxCollider2D.isTrigger = true;
                    buildIndicatorObject.SetActive(true);

                }
                break;
        }

        moduleState = newState;
    }

    void OnMouseDown()
    {
        if (moduleState == ModuleStateType.shop && builderScript.money > builderScript.basiShootCost)
        {
            builderScript.money -= builderScript.basiShootCost;
            ChaingeState(ModuleStateType.preview);

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("BuildOK"))
        {
            buildOK = true;
        }

        if (other.CompareTag("Station"))
        {
            stationTouching = true;
        }
            
     }
}



