using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    float distance = 10.0f;
    [SerializeField]
    float speed = 0.5f;

    int direction = 1;
    float deltaDist;
    float totalDist;

    Rigidbody player;

    void Start () {
        deltaDist = 0.0f;
        totalDist = 0.0f;
        player = null;
    }

    void FixedUpdate() {
        deltaDist = Time.deltaTime * speed;
        transform.position = transform.position + new Vector3(0.0f, 0.0f, deltaDist * direction);
        if (player != null)
            player.MovePosition(player.position + new Vector3(0.0f, 0.0f, deltaDist * direction));
        totalDist += deltaDist;
        if (totalDist >= distance)
        {
            direction *= -1;
            totalDist = 0.0f;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        player = col.gameObject.GetComponent<Rigidbody>();
        //Debug.Log("COLLISION");
    }

    void OnCollisionExit(Collision col)
    {
        player = null;
    }
}
