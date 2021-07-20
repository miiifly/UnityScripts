using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float movementSpeed;
    


    void Start()
    {
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        GetComponent<Rigidbody>().AddForce(Vector3.right * 1000, ForceMode.Impulse);
    }
    void Update()
    {
       
    }

   
}
