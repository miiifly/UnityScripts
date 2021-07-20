using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SpaceObject : GravitationalForces


{
    public InputValute iv;

    #region variables
    [Header("Space Object properties")]

    [SerializeField]
    private bool _onlyUseGravity = true;

    [Header("Space Object properties")]
    [SerializeField]
    public double _mass = 0;

    #endregion

    public bool OnlyUseGravity
    {
        get { return _onlyUseGravity; }
        set { _onlyUseGravity = value; }
    }

    #region Unity functions

    void FixedUpdate()
    {
        if(iv.chek == true){
            if (!_onlyUseGravity)
                return;

            if (!initializationComplete)
                Init();

            ApplyGravity();
        }
    }

    void OnDrawGizmosSelected() //Debugging in editor
    {
        if (_spaceObjects != null)
        {
            Gizmos.color = Color.gray;

            foreach (var spaceObject in _spaceObjects)
            {
                Gizmos.DrawLine(transform.position, spaceObject.transform.position);
            }
        }
    }
    #endregion

    /// <summary>
    /// Get gravitational pull force from this object, depending on the distance from it
    /// </summary>
    /// <param name="distanceMetersFromObject"> distance between 2 objects in meters </param>
    /// <returns></returns>
    public double GetGravitationalPullForce(double distanceMetersFromObject)
    {
        return Constants.GRAVITATIONAL_CONSTANT * _mass / Math.Pow(distanceMetersFromObject, 2);
    }

    /// <summary>
    /// Get the needed velocity to have a stable circular orbit around a gravitational object
    /// </summary>
    /// <param name="distanceMetersFromObject"> distance between 2 objects in meters </param>
    /// <returns></returns>
    public double GetVelocityForCircularOrbit(double distanceMetersFromObject)
    {
        return Math.Sqrt(Constants.GRAVITATIONAL_CONSTANT * _mass / distanceMetersFromObject);
    }

    /// <summary>
    /// Get the needed velocity to escape a gravitational object's gravity
    /// </summary>
    /// <param name="distanceMetersFromObject"> distance between 2 objects in meters </param>
    /// <returns></returns>
    public double GetEscapeVelocity(double distanceMetersFromObject)
    {
        return Math.Sqrt(2 * Constants.GRAVITATIONAL_CONSTANT * _mass / distanceMetersFromObject);
    }
}