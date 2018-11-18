using UnityEngine;

public enum Status
{
    UP,
    DOWN,
    DISABLED
};

public class Button : MonoBehaviour {

    
    public Status Status { get; set; }
    
    public Door Door
    {
        get { return _Door; }
        set { _Door = value; }
    }
    [SerializeField]
    Door _Door;

    void Start () {
        Status = Status.UP;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && Status == Status.UP)
        {
            Status = Status.DOWN;
            transform.position = new Vector3( transform.position.x, transform.position.y - 0.15f, transform.position.z);
            Door.OpenDoor(this);
        }
    }
}
