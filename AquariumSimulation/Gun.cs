using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    private GameObject target;

    private bool targetLocked;

    public GameObject turretTopPart;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;


    public float fireTimer;
    private bool shotReady;




    private void Start()
    {
        shotReady = true;
    }

    private void Update()
    {




        if (targetLocked)
        {
            turretTopPart.transform.LookAt(target.transform);
            turretTopPart.transform.Rotate(0,0, 0);

            if (shotReady)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity);
        _bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        
        Vector3 forceDirection = (bullet.transform.position - target.transform.position ).normalized;
        _bullet.GetComponent<Rigidbody>().AddForce(_bullet.transform.forward * Time.deltaTime * 375000, ForceMode.Impulse);
        shotReady = false;
        StartCoroutine(FireRate());
        targetLocked = false;
    }
    public IEnumerator FireRate()
    {
        yield return new WaitForSeconds(fireTimer);
        shotReady = true;

    }
    void OnTriggerEnter(Collider other)
    {



        if (other.tag == "Enemy")
        {
            target = other.gameObject;
            targetLocked = true;
        }
    }
}
