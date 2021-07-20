using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class background : MonoBehaviour
{
    public List<GameObject> list = new List<GameObject>();
    public static Camera cam;
    private float HalfobjLenght;
    private float ScreenBoundLeft;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        HalfobjLenght = list[0].GetComponent<Renderer>().bounds.extents.x;
        cam = Camera.main;
        ScreenBoundLeft = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, cam.transform.position.z)).x;
       
        
    }

    // Update is called once per frame
    void LateUpdate()
    {  
        for (int i=0;i<list.Count;i++)
        {
            list[i].transform.Translate( Time.fixedDeltaTime * -2.5f,0,0 ); 
            //sprawdzanie czy obiekt wychodzi z lewej poza kamerę
            if ((list[i].transform.position.x + HalfobjLenght) <= ScreenBoundLeft)
            {
                float diff = ( Mathf.Abs(ScreenBoundLeft - (list[i].transform.position.x + HalfobjLenght  )));
                list[i].transform.position = new Vector3( list[i].transform.position.x + HalfobjLenght * 4f ,list[i].transform.position.y,list[i].transform.position.z);
                
            }
            
         
           
        }
       
    }

}
