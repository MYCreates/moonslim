using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int ScoreVal { get; private set; } = 0;
    Text scoreText;

    [SerializeField]
    public GameObject Death;


    void Start () {
        scoreText = GetComponent<Text>();	
	}

    void Update()
    {
        scoreText.text = "Precious stone: " + ScoreVal;
    }

    public void StoneColleccted()
    {
        ScoreVal++;
    }
    public void EktoHit()
    {
        ScoreVal--;
    }
}
