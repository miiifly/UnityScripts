using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart2 : MonoBehaviour
{
    public GameObject completeLeveUI;

    void Start()
    {
        
    } 

    public void CompleteLevel()
    {
        completeLeveUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
        }
    }
    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player2")
        {
            CompleteLevel();

        }
        else
        {

        }
    }
}
