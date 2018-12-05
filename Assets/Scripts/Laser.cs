using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laser;
    private Transform ekto;

    [SerializeField]
    public Color ColorLaser = Color.red;
    [SerializeField]
    public float RangeLaser = 25.0f;
    [SerializeField]
    public float LaserTime = 1.0f;
    private float beginTime = 0.0f;

    [SerializeField]
    AudioClip audioClip;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        ekto = FindObjectOfType<PlayerController>().transform;
        laser = GetComponent<LineRenderer>();
        laser.material.color = ColorLaser;
        beginTime = Time.time;
        source.PlayOneShot(audioClip, 0.5f);
    }

    void Update()
    {
        if (Time.time - beginTime > LaserTime) Destroy(gameObject);

        Vector3 dir = new Vector3(0f, 0f, ekto.position.z - transform.position.z);
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;

        laser.SetPosition(0, ray.origin);
        if (Physics.Raycast(ray, out hit, RangeLaser))
        {
            if (hit.collider.CompareTag("Destroyable")) Destroy(hit.collider.gameObject);

            if (hit.collider.CompareTag("Player")) hit.collider.gameObject.GetComponent<PlayerController>().Kill();

            if (hit.collider)
            {
                laser.SetPosition(1, hit.point);
            }
        }
        else laser.SetPosition(1, ray.GetPoint(RangeLaser));
    }
}
