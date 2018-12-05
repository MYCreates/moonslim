using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    [SerializeField]
    GameObject winCanvas;

    [SerializeField]
    GameObject deathCanvas;

    [SerializeField]
    AudioClip audioClipWin;
    [SerializeField]
    AudioClip audioClipDeath;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            End();
    }

    private void End()
    {
        winCanvas.SetActive(true);
        source.PlayOneShot(audioClipWin, 0.5f);
        Time.timeScale = 0;
        FindObjectOfType<ButtonManager>().pauseAvailable = false;
    }

    public void Dead()
    {
        deathCanvas.SetActive(true);
        source.PlayOneShot(audioClipDeath, 0.5f);
        FindObjectOfType<ButtonManager>().pauseAvailable = false;
    }
}
