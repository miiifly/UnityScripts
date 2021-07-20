
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    public Text scoreText;
    public int scr = 0 ;
    
    void Start()
    {
        scoreText.text = scr.ToString();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
        {
            scr++;
            scoreText.text = scr.ToString();
        }
    }
}
