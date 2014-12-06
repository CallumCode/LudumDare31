using UnityEngine;
using System.Collections;

public class BarScript : MonoBehaviour 
{
    public float minLength = 0.1f;
    public float maxLength = 0.4f;

    public Transform objectToFollow;
	// Use this for initialization
	void Start () 
    {
 	}
	
    // Update is called once per frame
	void Update ()
    {
        if (objectToFollow != null)
        {
            transform.position = objectToFollow.position + new Vector3(0, transform.localScale.y , 0) - new Vector3(transform.localScale.x * 0.5f , 0 , 0);
        }
 	}

    public void SetValue(float t)
    {
        t = Mathf.Clamp01(t);
        float x = Mathf.Lerp(minLength, maxLength, t);
        float y = transform.localScale.y;
        float z = transform.localScale.z;

        transform.localScale = new Vector3(x , y , z);  
     }
}
