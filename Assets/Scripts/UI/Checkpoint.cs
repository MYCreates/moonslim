using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    private int checkpointNumber;

    private ButtonManager buttonManager;

    void Start () {
        buttonManager = FindObjectOfType<ButtonManager>();
	}

    void Update() { }

    void OnTriggerEnter( Collider col)
    {
        Debug.Log("Triggered");
        buttonManager.checkpoint = checkpointNumber;
    }
    
}
