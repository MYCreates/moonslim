using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    [SerializeField]
    GameObject winCanvas;

    [SerializeField]
    GameObject deathCanvas;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            End();
    }

    private void End()
    {
        winCanvas.SetActive(true);
        Time.timeScale = 0;
        FindObjectOfType<ButtonManager>().pauseAvailable = false;
    }

    public void Dead()
    {
        deathCanvas.SetActive(true);
        FindObjectOfType<ButtonManager>().pauseAvailable = false;
    }
}
