using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform player;
    private float camSpeed = 5f;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, 0) * Time.deltaTime * camSpeed;
    }
}
