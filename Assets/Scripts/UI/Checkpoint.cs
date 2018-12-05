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

    void OnTriggerEnter( Collider col)
    {
        buttonManager.checkpoint = checkpointNumber;
    }
    
}
