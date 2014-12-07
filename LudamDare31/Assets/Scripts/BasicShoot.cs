using UnityEngine;
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

 
    // Use this for initialization
    void Start()
    {
        Init();
    }


    protected virtual void Init()
    {
        Physics2D.IgnoreLayerCollision(8, 10);

        hpBarObject = Instantiate(BarPrefab, transform.position, transform.rotation) as GameObject;
        hpBarScript = hpBarObject.GetComponentInChildren<BarScript>();
        hpBarScript.objectToFollow = transform;
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

    }
    void ShopUpdate()
    {

    }

    void BuiltUpdate()
    {
        ShootAtMouse();

        if (health <= 0)
        {
            DestroySelf();
        }
    }

    void ShootAtMouse()
    {
        if (Input.GetButtonDown("Fire1") && (Time.time > (fireRateTimer + 1 / fireRate)))
        {
            fireRateTimer = Time.time;

            audio.Play();

            clickPoint = Input.mousePosition;

            Vector3 dir = clickPoint - Camera.main.WorldToScreenPoint(transform.position);
            dir.Normalize();
            Quaternion ori = Quaternion.LookRotation(Vector3.forward, dir);
            Instantiate(projectileObject, transform.position, ori);
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
            Destroy(coll.gameObject);
        }


        if (coll.gameObject.CompareTag("Ship"))
        {
            BasicShip basicShip = coll.gameObject.GetComponent<BasicShip>();
            TakeDamage(basicShip.crashDamage);
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
        Destroy(hpBarObject);
        Destroy(gameObject);
    }

    void ChaingeState(ModuleStateType newState)
    {
        switch (newState)
        {
            case ModuleStateType.preview:
                {
                    Color color = renderer.material.color;
                    color.a = 0.5f;
                    renderer.material.color = color;

                }
                break;
            case ModuleStateType.built:
                {
                    Color color = renderer.material.color;
                    color.a = 1;
                    renderer.material.color = color;
                }
                break;
            case ModuleStateType.shop:
                {

                }
                break;
        }

        moduleState = newState;
    }
}


