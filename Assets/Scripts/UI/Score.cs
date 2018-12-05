using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int ScoreVal { get; private set; } = 0;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text scorePopUp;

    float displayTime = 2f;
    float time = 0f;

    Vector3 initPos; 

    void Start () {
        initPos = scorePopUp.transform.position;
	}

    void Update()
    {
        scoreText.text = ScoreVal.ToString();
        time -= Time.deltaTime;
        if (time <= 0)
        {
            scorePopUp.transform.position = initPos;
            scorePopUp.text = "";
        } else
        {
            scorePopUp.transform.position = initPos + new Vector3(0f, (displayTime - time) * 20, 0f);
        }
    }

    public void StoneCollected()
    {
        ScoreVal++;
        scorePopUp.text = "+1";
        scorePopUp.color = Color.green;
        time = displayTime;
    }
    public void EktoHit()
    {
        ScoreVal--;
        scorePopUp.text = "-1";
        scorePopUp.color = Color.red;
        time = displayTime;
    }
}
