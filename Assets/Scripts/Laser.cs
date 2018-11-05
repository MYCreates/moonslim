using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer laser;
    private Transform ekto;

    [SerializeField]
    public float RangeLaser = 25.0f;
    [SerializeField]
    public float LaserTime = 1.0f;
    private float beginTime = 0.0f;

    void Start()
    {
        ekto = GameObject.FindGameObjectWithTag("Player").transform;
        laser = GetComponent<LineRenderer>();
        laser.material.color = Color.red;
        beginTime = Time.time;
    }

    void Update()
    {
        if (Time.time - beginTime > LaserTime) Destroy(gameObject);

        Vector3 dir = ekto.position - transform.position;
        dir.y = 0;
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;

        laser.SetPosition(0, ray.origin);
        if (Physics.Raycast(ray, out hit, RangeLaser))
        {
            if (hit.collider.tag == "Destroyable") Destroy(hit.collider.gameObject);

            if (hit.collider.tag == "Player") hit.collider.gameObject.GetComponent<PlayerControler>().Kill();

            if (hit.collider)
            {
                laser.SetPosition(1, hit.point);
            }
        }
        else laser.SetPosition(1, ray.GetPoint(RangeLaser));
    }
}
