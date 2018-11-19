using System;
using UnityEngine;

public class CheeseWall : MonoBehaviour {

	void Start()
	{


	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Player") return;
		gameObject.SetActive(false);    
	}
}