using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] public float decayHeight;
    public float assignedFallingSpeed;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void AssignFallingSpeed(float fallingSpeed)
	{
		assignedFallingSpeed = fallingSpeed * 0.015f;
	}

    public void MoveUnit()
    {
        transform.position += Vector3.down * assignedFallingSpeed;
    }

    public void DestroyUnit()
    {
        Destroy(this);
    }
}