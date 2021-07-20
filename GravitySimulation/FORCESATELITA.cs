using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FORCESATELITA : MonoBehaviour
{

    public Vector3 Force;
    public Vector3 Force2; 
    public Vector3 Force3;
    public float timeforce;
    private bool a=false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Force);
        StartCoroutine(FireRate());
        
    }


    public IEnumerator FireRate()
    {
        yield return new WaitForSeconds(timeforce);

        a = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (a == true)
        {
            GetComponent<Rigidbody>().AddForce(Force3);
            a = false;
        }
    }
}
