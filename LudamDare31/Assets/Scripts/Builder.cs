using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour
{
    public GameObject BasicShooter;

    public Transform shopTransform;
    public Transform startTransform;

    int moduleCounter = 0;
	// Use this for initialization
	void Start () 
    {
        GameObject startObject = Instantiate(BasicShooter, startTransform.position, Quaternion.identity) as GameObject;
        BasicShoot startScript = startObject.GetComponent<BasicShoot>();
        startScript.Init(this);
        startScript.ChaingeState(BasicShoot.ModuleStateType.built);

        RestockBasicShooter();
            
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

   public void RestockBasicShooter()
    {
        GameObject shopObject = Instantiate(BasicShooter, shopTransform.position, Quaternion.identity) as GameObject;
        BasicShoot shopScript = shopObject.GetComponent<BasicShoot>();
        shopScript.Init(this);
        shopScript.ChaingeState(BasicShoot.ModuleStateType.shop);
        moduleCounter++;
    }

   public void ModuleLost()
   {
       moduleCounter--;

       if (moduleCounter <= 0)
       {
           Application.LoadLevel("Menu");
       }
   }
}
