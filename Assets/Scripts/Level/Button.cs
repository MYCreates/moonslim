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

    void OnTriggerEnter(Collider other)
    {
        if (Status == Status.UP)
        {
            Status = Status.DOWN;
            transform.position = new Vector3(0.0f, 0.16f, 5.19f);
            Door.OpenDoor(this);
        }
    }
}
