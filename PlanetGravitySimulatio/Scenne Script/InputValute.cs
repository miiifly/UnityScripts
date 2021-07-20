using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputValute : MonoBehaviour
{
    public SpaceObject so;
    public SpaceObject se;
   
    public GameObject go;
    public InputField ValueDouble1;
    public InputField ValueDouble2;
    public InputField ValueDouble3;
    public InputField ValueDouble4;
    public InputField ValueDouble5;
    public GameObject VD1;
    


    double cms;
    double cvs;
    double cme;
    double cve;
    float dis;
    float time = Time.timeScale;

    public bool chek = false;
    public string namescane;

    Vector3 distanc;
    void Awake()
    {
        Time.timeScale = 0;


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&chek==false)
        {
            Time.timeScale = 1;
            VD1.SetActive(false);
            chek = true;
          
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(namescane);
        }

    }

    // Update is called once per frame
    public void ChangeMassSun()
    {
        cms = Double.Parse(ValueDouble1.text, CultureInfo.InvariantCulture);
        so._mass = cms;

    }

    public void ChangeVelocitySun()
    {
        cvs = Double.Parse(ValueDouble2.text, CultureInfo.InvariantCulture);
        so._initialVelocity = cvs;
    }
    public void ChangeMassEarth()
    {
        cme = Double.Parse(ValueDouble3.text, CultureInfo.InvariantCulture);
        se._mass = cme;

    }

    public void ChangeVelocityEarth()
    {
        cve = Double.Parse(ValueDouble4.text, CultureInfo.InvariantCulture);
        se._initialVelocity = cve;
    }
    public void ChangeDistance()
    {
        dis = float.Parse(ValueDouble5.text, CultureInfo.InvariantCulture);
        distanc.x = dis / 600000;
        go.transform.position = distanc;
    }




}
