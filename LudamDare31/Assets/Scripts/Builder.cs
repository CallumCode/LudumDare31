using UnityEngine;
using System.Collections;
 using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    public GameObject BasicShooter;

    public Transform shopTransform;
    public Transform startTransform;

    int moduleCounter = 0;

    public ArrayList modulesList;

    public float money = 0;
    public float shipBounty = 10;
    public GameObject moneyTextObject;
    Text moneyText;
    float moneyRate = 5;
    public float basiShootCost = 100;

    // Use this for initialization
	void Start () 
    {
        modulesList = new ArrayList();

        GameObject startObject = Instantiate(BasicShooter, startTransform.position, Quaternion.identity) as GameObject;
        BasicShoot startScript = startObject.GetComponent<BasicShoot>();
        startScript.Init(this );
        startScript.ChaingeState(BasicShoot.ModuleStateType.built);
      
        modulesList.Add(startObject);
        RestockBasicShooter();

        moneyText = moneyTextObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        money += moneyRate * Time.deltaTime;
        moneyText.text = "Money " + Mathf.Round(money);
	}

   public void RestockBasicShooter()
    {
        GameObject shopObject = Instantiate(BasicShooter, shopTransform.position, Quaternion.identity) as GameObject;
        BasicShoot shopScript = shopObject.GetComponent<BasicShoot>();
        shopScript.Init(this);
        shopScript.ChaingeState(BasicShoot.ModuleStateType.shop);
        moduleCounter++; 
    }

   public void ModuleLost(int index)
   {
       moduleCounter--;

       if (index != -1)
       {
           modulesList.RemoveAt(index);
           UpdateIndexs();
       }

       if (moduleCounter <= 0)
       {
           Application.LoadLevel("Menu");
       }
   }

   void UpdateIndexs()
   { 
        for(int i = 0; i < modulesList.Count ; i++)
       {
           GameObject mod = (GameObject)modulesList[i];
           if (mod != null) mod.GetComponent<BasicShoot>().listIndex = i; 
       }
   }

    public   Vector3 GetAvgPos()
   {
       Vector3 pos = Vector3.zero;
       float count = 1;

       if (modulesList != null)
       {
           for (int i = 0; i < modulesList.Count; i++)
           {
               GameObject mod = (GameObject)modulesList[i];
               if (mod != null) pos += mod.transform.position;
               count++;
           }

       }
        pos = pos/ count;
       return pos;

   }
}
