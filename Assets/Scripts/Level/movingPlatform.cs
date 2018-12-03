using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour {

    [SerializeField]
    float distance = 10.0f;
    [SerializeField]
    float speed = 0.5f;

    int direction = 1;
    Rigidbody _Rb;

    // Use this for initialization
    void Start () {
        _Rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        _Rb.MovePosition(new Vector3(0.0f, 0.0f, Time.deltaTime * speed));
	}
}
