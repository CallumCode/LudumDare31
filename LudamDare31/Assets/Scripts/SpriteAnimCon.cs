using UnityEngine;
using System.Collections;

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]

 
public class SpriteAnimCon : MonoBehaviour 
{
    Animator anim;
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () 
    {
         anim.SetFloat("Speed",  rigidbody2D.velocity.magnitude);
	}

  public  float CallDeathAnimation()
    {
        float length = 1;

        anim.SetTrigger("Death");

        length = anim.GetCurrentAnimationClipState(0).Length;

        return length;
    }   



}

