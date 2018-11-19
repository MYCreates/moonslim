using UnityEngine;

public class Stone : MonoBehaviour {

    Score score;
    void Start()
    {
        score = FindObjectOfType<Score>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        score.StoneColleccted();
        // TODO : Anim + SON
        gameObject.SetActive(false);    
    }
}
