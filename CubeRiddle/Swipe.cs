using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine.UI;
using System;

public class Swipe : MonoBehaviour
{
    public float RollSpeed;
    public bool isRolling;
    public int vc1;
    public int vc2;
    public int vc3;
    public int vc4;
    public Text scoreText;
    public int scr = 0;
    public GameObject player;
    private AudioSource _sor;
    public AudioSource EndGame;
    public bool a = true;
    private int up = 0;
    private int down = 0;
    private int left = 0;
    private int right = 0;

    void Start()
    {
        scoreText.text = scr.ToString();
        _sor = GetComponent<AudioSource>();
    }

    public void Update()
    {

        if (vc1 != 2 && vc2 != 2 && vc3 != 2 && down==0 )
        {
            if (isRolling)
                return;

            else if (Input.GetKeyDown(KeyCode.S))
            {
                scr++;
                scoreText.text = scr.ToString();
                isRolling = true;
                StartCoroutine(Roll(transform.position + new Vector3(-12f, -12f, 0), Vector3.forward));
                _sor.Play ();
               
                if (vc2 == 1) vc2--;
            
                else vc1++;
                
            }


            else if (Input.GetKeyDown(KeyCode.W))
            {
                scr++;
                scoreText.text = scr.ToString();
                isRolling = true;
                StartCoroutine(Roll(transform.position + new Vector3(12f, -12f, 0), Vector3.back));
                _sor.Play();
            

                if (vc1 == 1) vc1--;
                else vc2++;
            }

            else if (Input.GetKeyDown(KeyCode.A))
            {
                scr++;
                scoreText.text = scr.ToString();
                isRolling = true;
                StartCoroutine(Roll(transform.position + new Vector3(0, -12f, 12f), Vector3.right));
                _sor.Play();
               
                if (vc4 == 1) vc4--;
                else vc3++;
            }


            else if (Input.GetKeyDown(KeyCode.D))
            {
                scr++;
                scoreText.text = scr.ToString();
                isRolling = true;
                StartCoroutine(Roll(transform.position + new Vector3(0, -12f, -12f), Vector3.left));
                _sor.Play();
             
                if (vc3 == 1) vc3--;
                else vc4++;
            }
                    
            
        }

        if (vc1 == 2 || vc2 == 2 || vc3 == 2 || vc4 == 2 )
        {
            if (a == true)
            {
                EndGame.Play();
                a = false;
            }
            PlayerLose();
            
        }
       
        
       
    }

    void PlayerLose()
    {
        Rigidbody rigidbody = player.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
    }
    
    


    IEnumerator Roll(Vector3 pivot,Vector3 _vector)
    {
        for(int i =0; i < (90/RollSpeed); i++)
        {
            transform.RotateAround(pivot, _vector, RollSpeed);
            yield return new WaitForSeconds(0.05F);
        }


        isRolling = false;
        yield return null;
    }
}
