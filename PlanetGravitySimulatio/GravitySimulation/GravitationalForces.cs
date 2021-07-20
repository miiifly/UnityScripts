using System;
using UnityEngine;
using System.Linq;

public class GravitationalForces : MonoBehaviour
{
    protected const int FREE_VELOCITY = 0;
    protected const int CIRCULAR_ORBIT_VELOCITY = 1;
    protected const int ESCAPE_ORBIT_VELOCITY = 2;

    protected enum InitialVelocity
    {
        Free = FREE_VELOCITY,
        CircularOrbit = CIRCULAR_ORBIT_VELOCITY,
        EscapeOrbit = ESCAPE_ORBIT_VELOCITY
    }

    [Header("Velocity configuration")]

    [SerializeField]
    [Tooltip("Auto calculate velocity of the object: \n" +
        " - Free: No constrain. \n" +
        " - Circular: Orbit around closest massive object. \n" +
        " - Escape: Leave orbit from closest massive object.")]
    protected InitialVelocity _velocityType;

    [SerializeField]
    [Tooltip("Velocity in km/s")]
    public double _initialVelocity = 0;//meters/second
    [SerializeField]
    protected Vector3 _initialDirection = new Vector3();

    //[Header("Debug")]
    //[SerializeField]
    protected GameObject[] _spaceObjects;
    protected Rigidbody _rigidbody;
    protected GameManager _gameManager;

    protected bool initializationComplete = false;

    protected void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spaceObjects = GameObject.FindGameObjectsWithTag("GravitationalSpaceObject").Where(o => o.gameObject != gameObject).ToArray();

        _initialVelocity = GetVelocity((int)_velocityType);
        _rigidbody.velocity = _initialDirection * (float)_initialVelocity * Constants.KM_TO_METERS * _gameManager.TimeScale / _gameManager.SpaceScaleMeters;

        initializationComplete = true;
    }

    protected void ApplyGravity()
    {
        var gravityForces = new Vector3();

        foreach (var spaceObject in _spaceObjects)
        {
            gravityForces += GetGravity(spaceObject) * Time.fixedDeltaTime;
        }

        _rigidbody.velocity += gravityForces;
    }

    protected Vector3 GetGravity(GameObject spaceObject)
    {
        var direction = spaceObject.transform.position - transform.position;
        var distance = direction.magnitude * _gameManager.SpaceScaleMeters;

        var gravity = spaceObject.GetComponent<SpaceObject>().GetGravitationalPullForce(distance);
        var gravityScaled = (float)(gravity * Math.Pow(_gameManager.TimeScale, 2) / _gameManager.SpaceScaleMeters);

        return direction.normalized * gravityScaled;
    }

    private double GetVelocity(int velocityType)
    {
        if (_spaceObjects.Length == 0)
        {
            throw new SystemException("No spaceObjects found. Needed at least 1 to get velocity");
        }

        var target = _spaceObjects
          .OrderByDescending(o => o.GetComponent<SpaceObject>().GetGravitationalPullForce(
          (o.transform.position - transform.position).magnitude) * _gameManager.SpaceScaleMeters)
          .First();

        var distance = (target.transform.position - transform.position).magnitude * _gameManager.SpaceScaleMeters;
        var velocity = 0.0;

        switch (velocityType)
        {
            case CIRCULAR_ORBIT_VELOCITY:
                velocity = target.GetComponent<SpaceObject>().GetVelocityForCircularOrbit(distance) / 1000 + target.GetComponent<Rigidbody>().velocity.magnitude;
                break;
            case ESCAPE_ORBIT_VELOCITY:
                velocity = target.GetComponent<SpaceObject>().GetEscapeVelocity(distance) / 1000;
                break;
            default: //Free velocity
                Debug.Log("FREE_VELOCITY (" + _velocityType + ")");
                velocity = _initialVelocity;
                break;
        }

        return velocity;
    }
}