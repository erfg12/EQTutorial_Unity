using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    float speedH = 2.0f;
    float speedV = 2.0f;
    public float yaw = 0.0f; // y
    public float pitch = 0.0f; // x

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.PageUp))
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, CamPitch, step);
            Debug.Log("look up");
        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, CamPitch, step);
            Debug.Log("look down");
        }

        // right click
        if (Input.GetMouseButton(1)) {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            // adjust player
            player.transform.eulerAngles = new Vector3(0, yaw, 0.0f);
        } else {
            yaw = transform.rotation.eulerAngles.y;
            pitch = transform.rotation.eulerAngles.x;
        }

        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, 0);
    }
}
