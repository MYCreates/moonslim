using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour {

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        InvokeRepeating("UpdateEverySecond", 0, 0.5f);
    }

    void UpdateEverySecond()
    {
        text.fontSize = (text.fontSize == 75) ? 70 : 75;
	}
}
