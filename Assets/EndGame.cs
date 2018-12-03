using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    [SerializeField]
    GameObject winCanvas;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            End();
    }

    private void End()
    {
        winCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}
