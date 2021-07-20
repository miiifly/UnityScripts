using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatewithmouse : MonoBehaviour
{
    public float rotSpeed = 30f;

    float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < 0f) _angle = 360 + _angle;
        if (_angle > 180f) Mathf.Max(_angle, 360 + _min);
        return Mathf.Min(_angle, _max);
    }

    void RotateGameObject()
    {
        float h = Input.GetTouch(0).deltaPosition.x * Time.deltaTime * rotSpeed * Mathf.Deg2Rad;
        float v = Input.GetTouch(0).deltaPosition.y * Time.deltaTime * rotSpeed * Mathf.Deg2Rad;

        Vector3 rot = transform.rotation.eulerAngles + new Vector3(-v, h, 0f);
        //Change the y & z values to match your expected behaviour.
        rot.x = ClampAngle(rot.x, -5f, 20f);
        //Clamp rotation on the y-axis
        rot.y = ClampAngle(rot.y, -20f, 20f);
        transform.eulerAngles = rot;
    }
}
