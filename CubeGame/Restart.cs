using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            Application.LoadLevel(scene.name);

        }
        else
        {
            
        }
    }
    }
