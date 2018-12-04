using UnityEngine;

public class Cheese : MonoBehaviour {

    [SerializeField]
    AudioClip audio;
    AudioSource source;

    [SerializeField]
    float speedBoost = 2.0f;

    [SerializeField]
    float boostTime = 3.0f;

    float disabledTime = 0.0f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        // Anim 
        if (disabledTime > 0)
        {
            disabledTime -= Time.deltaTime;
            if (disabledTime <= 0)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        source.PlayOneShot(audio, 0.5f);
        col.GetComponent<PlayerController>().EatCheese(speedBoost, boostTime);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        disabledTime = boostTime + 1.0f;
    }
}