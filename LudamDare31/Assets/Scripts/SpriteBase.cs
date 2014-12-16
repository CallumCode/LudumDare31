using UnityEngine;
using System.Collections;

 [RequireComponent(typeof(Rigidbody2D))]
 [RequireComponent(typeof(SpriteAnimCon))]
 [RequireComponent(typeof(AudioSource))]

public class SpriteBase : MonoBehaviour 
{
	const float maxHealth = 100;
	float health = maxHealth;

    float force = 15;
    float rotateSpeed = 20;

    public GameObject BarPrefab = null;


    public GameObject hpTransform;


    SpriteAnimCon spriteAnimCon;

    BarScript hpBarScript;


    protected  AudioSource deathSound;


 	// Use this for initialization
	void Start ()
    {
         
           
            
	}

    void GetSounds()
    {
        AudioSource[] AudioSources = GetComponents<AudioSource>();

        deathSound = AudioSources[0];

    }

    protected virtual void Init()
    {
        spriteAnimCon = GetComponent<SpriteAnimCon>();

        if (BarPrefab != null)
        {
            HitPointBar();
        }

    }

    void HitPointBar()
    {
        GameObject hpBarObject = Instantiate(BarPrefab, hpTransform.transform.position, Quaternion.identity) as GameObject;
        hpBarScript = hpBarObject.GetComponentInChildren<BarScript>();
        hpBarScript.objectToFollow = hpTransform.transform;
    }

	
	// Update is called once per frame
	void Update () 
    {
        TestControls();

	}

     void TestControls()
     {

        if(Input.GetKey(KeyCode.W))
        {

            transform.rigidbody2D.AddForce(transform.up * Time.deltaTime * force);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-transform.forward, rotateSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            StartDeath();
        }
      
     }


     protected void StartDeath()
     {
         float length = spriteAnimCon.CallDeathAnimation();
         Invoke("EndDeath", length);
     }

     void EndDeath()
     {
         Destroy(gameObject);
     }


     public void TakeDamage(float amount)
     {

         health -= amount;
         health = Mathf.Clamp(health , 0, maxHealth);
     }
}
