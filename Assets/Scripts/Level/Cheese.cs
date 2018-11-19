using UnityEngine;

public class Cheese : MonoBehaviour {

    [SerializeField]
    float speedBoost = 2.0f;

    [SerializeField]
    float boostTime = 3.0f;

    float disabledTime = 0.0f;

    // Use this for initialization
    void Update()
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
        if (col.tag != "Player") return;
        col.GetComponent<PlayerController>().EatCheese(speedBoost, boostTime);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        disabledTime = boostTime + 1.0f;
    }
}