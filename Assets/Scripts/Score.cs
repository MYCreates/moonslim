using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    [HideInInspector]
    public int ScoreVal = 0;
    Text scoreText;

	void Start () {
        scoreText = GetComponent<Text>();	
	}
	
	void Update () {
        scoreText.text = "Precious stone: " + ScoreVal;	
	}
}
