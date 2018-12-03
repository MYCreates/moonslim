using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int ScoreVal { get; private set; } = 0;
    Text scoreText;

    void Start () {
        scoreText = GetComponent<Text>();	
	}

    void Update()
    {
        scoreText.text = "Precious stone: " + ScoreVal;
    }

    public void StoneCollected()
    {
        ScoreVal++;
    }
    public void EktoHit()
    {
        ScoreVal--;
    }
}
