using UnityEngine;

public class FinalDoor : MonoBehaviour {

    [SerializeField]
    public int NumberOfStonesToOpen;
    Door door;
    Score score;

    void Start () {
        door = GetComponentInChildren<Door>();
        score = FindObjectOfType<Score>();
    }
	
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && score.ScoreVal >= NumberOfStonesToOpen)
        {
            door.OpenDoor();
        }   
    }
}
