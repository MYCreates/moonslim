using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform ekto;
    [SerializeField]
    public float offset = 10.0f;
	void Start () {
        ekto = FindObjectOfType<PlayerController>().transform;
	}
	
	void Update () {
        transform.position = new Vector3(ekto.position.x + offset, ekto.position.y, ekto.position.z);
	}
}
