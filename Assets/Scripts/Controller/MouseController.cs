﻿using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Transform ekto;
    [SerializeField]
    public bool hasLaser;
    [SerializeField]
    public Transform laserPrefab;
    private Transform laser;

    [SerializeField]
    public float maxLookDistance = 10.0f;
    [SerializeField]
    public float miniDistanceEkto = 2.0f;
    [SerializeField]
    public float fireRate = 5.0f;
    private float lastShotTime = 0.0f;
    [SerializeField]
    public float GrabDuration = 0.5f;

    private Rigidbody _Rb;

   void Start()
   {
        ekto = FindObjectOfType<PlayerController>().transform;
        _Rb = GetComponent<Rigidbody>();
        if (hasLaser)
            laser = transform.GetChild(0);

    }

    void Update()
    {
        float distance = Vector3.Distance(ekto.position, transform.position);
        Vector3 dir = ekto.position - transform.position;
        if (distance <= maxLookDistance && dir.x == 0)
        {
            LookAtEkto();
            if (hasLaser)
            {
                if ((Time.time - lastShotTime) > fireRate)
                {
                    Shoot();
                }
            }
        }  
    }

    void LookAtEkto()
    {
        Vector3 dir = ekto.position - transform.position;
        if (dir.z > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (hasLaser)
            {
                float xT = -Mathf.Abs(laser.transform.localPosition.x);
                laser.transform.localPosition = new Vector3(xT, laser.transform.localPosition.y, laser.transform.localPosition.z);
            }
            return;
        }
        GetComponent<SpriteRenderer>().flipX = false;
        if (hasLaser)
        {
            float xF = Mathf.Abs(laser.transform.localPosition.x);
            laser.transform.localPosition = new Vector3(xF, laser.transform.localPosition.y, laser.transform.localPosition.z);
        }
    }


    void Shoot()
    {
        //Reset the time when we shoot
        lastShotTime = Time.time;
        Instantiate(laserPrefab, laser.position, laser.rotation, transform);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            _Rb.constraints = RigidbodyConstraints.FreezeAll;
            // TO DO : GERER ICI L'ATTRAPAGE
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            _Rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
        }
    }
}
