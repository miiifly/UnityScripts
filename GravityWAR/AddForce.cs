﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{


    public Vector3 Force;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
