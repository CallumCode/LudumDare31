using UnityEngine;
using System.Collections;
using UnityEngine.UI;

 [RequireComponent(typeof(Rigidbody2D))]
 [RequireComponent(typeof(SpriteAnimCon))]
 [RequireComponent(typeof(AudioSource))]

public class SpriteBase : MonoBehaviour 
{
	const float maxHealth = 100;
    protected float health = maxHealth;

    float force = 15;
    float rotateSpeed = 20;
 
    SpriteAnimCon spriteAnimCon;
      
    protected  AudioSource deathSound;
    protected  Slider healthSlider;

 	// Use this for initialization
	void Start ()
    {

    


	}

    void GetSounds()
    {
        AudioSource[] AudioSources = GetComponents<AudioSource>();

        deathSound = AudioSources[0];

    }

    public virtual void Init()
    {

        spriteAnimCon = GetComponent<SpriteAnimCon>();

        healthSlider = GetComponentInChildren<Slider>();
        if (healthSlider == null)
        {
            Debug.Log("healthSlider is null");
            Debug.Break();
        }
        else
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.minValue = 0;
        }

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

         healthSlider.value = health;
     }

     void OnCollisionEnter2D(Collision2D coll)
     {
         DoCollision(coll);
     }

     protected virtual void DoCollision(Collision2D coll)
     {
   
     }

   
}
