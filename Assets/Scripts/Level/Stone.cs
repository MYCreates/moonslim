using UnityEngine;

public class Stone : MonoBehaviour {

    [SerializeField]
    AudioClip audioClip;
    AudioSource source;

    Score score;
    void Start()
    {
        source = GetComponent<AudioSource>();
        score = FindObjectOfType<Score>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        source.PlayOneShot(audioClip, 0.5f);
        score.StoneCollected();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
