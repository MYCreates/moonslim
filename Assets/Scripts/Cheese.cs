using UnityEngine;

public class Cheese : MonoBehaviour {

    [SerializeField]
    float speedBoost = 2.0f;

    [SerializeField]
    float boostTime = 3.0f;

    // Use this for initialization
    void Start () {
		// Anim 
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        col.GetComponent<PlayerControler>().EatCheese(speedBoost, boostTime);
        Destroy(gameObject);
    }
}