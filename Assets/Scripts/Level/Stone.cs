using UnityEngine;

public class Stone : MonoBehaviour {

    Score score;
    void Start()
    {
        score = FindObjectOfType<Score>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        score.StoneColleccted();
        // Anim
        gameObject.SetActive(false);    
    }
}
