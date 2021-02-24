using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrowdParentAgent : MonoBehaviour
{
    public GameObject soldier;
    public static int numSoldiers = 20;
    public static GameObject[] allSoldiers = new GameObject[numSoldiers];
    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = this.transform.gameObject;
        for(int i = 0; i < numSoldiers; i++)
        {
            // Instantiate numSoldiers (i.e. 20) number of Soldiers within a random area near our transform position
            allSoldiers[i] = (GameObject)Instantiate(soldier, parent.transform.position, Quaternion.identity);
            allSoldiers[i].transform.position = allSoldiers[i].transform.position + (Vector3) Random.insideUnitCircle * 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
