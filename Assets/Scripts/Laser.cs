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

    void Start()
    {
        ekto = FindObjectOfType<PlayerController>().transform;
        laser = GetComponent<LineRenderer>();
        laser.material.color = ColorLaser;
        beginTime = Time.time;
    }

    void Update()
    {
        print(Time.time);
        if (Time.time - beginTime > LaserTime) Destroy(gameObject);

        Vector3 dir = ekto.position - transform.position;
        dir.y = 0;
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
