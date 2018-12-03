using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eatable : MonoBehaviour {

    [SerializeField]
    private GameObject cheese;

	// Use this for initialization
	void Start () {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (int y = 0; y < 5; y++)
        {
            Instantiate(cheese, new Vector3(0, 0, 0), Quaternion.identity, transform);
            for (int z = 0; z < 5; z++)
            {
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
