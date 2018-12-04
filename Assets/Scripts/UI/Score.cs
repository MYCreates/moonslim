using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int ScoreVal { get; private set; } = 0;
    Text scoreText;

    void Start () {
        scoreText = GetComponentInChildren<Text>();	
	}

    void Update()
    {
        scoreText.text = ScoreVal.ToString() ;
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
