using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform ekto;
	void Start () {
        ekto = FindObjectOfType<PlayerController>().transform;
	}
	
	void Update () {
        transform.position = new Vector3(transform.position.x, ekto.position.y, ekto.position.z);
	}
}
