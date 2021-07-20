using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class NewBehaviourScript : MonoBehaviour
{
    private Touch touch;

    private Vector2 touchPosition;

    private Quaternion rotationX, rotationZ;

    private float titlSpeedModifier = 0.1f;


    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Moved:

                    rotationX = Quaternion.Euler(
                        -touch.deltaPosition.x * titlSpeedModifier,
                        0f,
                        0f);

                    transform.rotation = rotationX * transform.rotation;

                    rotationZ = Quaternion.Euler(
                        0f,
                        0f,
                        -touch.deltaPosition.y * titlSpeedModifier);

                    transform.rotation = transform.rotation * rotationZ;

                    break;
            }




        }




    }
}
