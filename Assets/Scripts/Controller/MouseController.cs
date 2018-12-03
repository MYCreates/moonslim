using System.Collections;
using UnityEngine;

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


    bool Grab { get; set; }
    float GrabTime { get; set; }
    float IFrame { get; set; }
    Vector3 ThrowDirection { get; set; }

    Animator _Animator { get; set; }
    Collider _Collider { get; set; }


    private Rigidbody _Rb;

   void Start()
   {
        ekto = FindObjectOfType<PlayerController>().transform;
        _Rb = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();
        _Collider = GetComponent<Collider>();
        if (hasLaser)
            laser = transform.GetChild(0);

    }

    void Update()
    {
        if (!ekto) return;
        float distance = Vector3.Distance(ekto.position, transform.position);
        Vector3 dir = ekto.position - transform.position;
        if (distance <= maxLookDistance && !(dir.x != 0.0f))
        {
            LookAtEkto();
            if (hasLaser)
            {
                if ((Time.time - lastShotTime) > fireRate)
                {
                    StartCoroutine(Shoot());
                }
            }
        }
        CheckGrab();
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


    IEnumerator Shoot()
    {
        int wait = 2;
        lastShotTime = Time.time + wait;
        _Animator.SetBool("Ready", true);
        yield return new WaitForSeconds(wait);
        _Animator.SetBool("Ready", false);
        //Reset the time when the mouse shoot
        Instantiate(laserPrefab, laser.position, laser.rotation, transform);
    }

    void CheckGrab()
    {
        if (IFrame > 0)
        {
            IFrame -= Time.deltaTime;
            if (IFrame <= 0)
            {
                _Collider.enabled = true;
            }
        }

        if (Grab)
        {
            if (GrabTime > 0)
            {
                GrabTime -= Time.deltaTime;
                ekto.GetComponent<Rigidbody>().velocity = transform.position - ekto.position;
            }
            else
            {
                // Lancer Ekto
                _Animator.SetBool("Grab", false);
                _Animator.SetBool("Launch", true);
                Grab = false;
                ekto.GetComponent<PlayerController>().HasControl = true;
                ekto.GetComponent<Rigidbody>().useGravity = true;
                ekto.GetComponent<Rigidbody>().velocity = ThrowDirection * 5f;
                IFrame = 0.75f;
            }
        }
        else
        {
            if (!_Animator) return;
            _Animator.SetBool("Launch", false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        
        _Rb.constraints = RigidbodyConstraints.FreezeAll;

        // Ekto grabbed
        if (IFrame <= 0)
        {
            // Collider & model
            _Collider.enabled = false;
            ekto.GetComponent<PlayerController>().Grab(transform.position);

            // Variables
            Grab = true;
            GrabTime = GrabDuration;
            _Animator.SetBool("Grab", true);

            // Direction du lancer
            if (_Collider.bounds.center.z - ekto.GetComponent<Collider>().bounds.center.z < 0)
                ThrowDirection = new Vector3(0, 1f, 1f);
            else
                ThrowDirection = new Vector3(0, 1f, -1f);
        }
        
    }

    private void OnCollisionExit(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        _Rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
    }
}
