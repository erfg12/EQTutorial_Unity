using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float jumpHeight = 4.0f;
    private float gravityValue = -9.81f;
    CharacterController controller;
    public bool UnderWater = false;
    public bool ClimbingLadder = false;

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float rotateSpeed = 3f;
        float speed = 8f;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && (groundedPlayer || UnderWater))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (UnderWater) {
            playerVelocity.y += Mathf.Sqrt(1.0f * gravityValue);
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        float curSpeed = speed * Input.GetAxis("Vertical");

        // Move forward / backward
        Vector3 forward = new Vector3(0,0,0);
        if (ClimbingLadder) {
            // convert forward/back to up/down
            forward = transform.TransformDirection(Vector3.up);
        } else {
            forward = transform.TransformDirection(Vector3.forward);
        }
        controller.SimpleMove(forward * curSpeed);
    }
    
    /*Rigidbody rb;
    public float jumpHeight = 7f;
    public bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 7.0f;

        transform.Rotate(0,x,0);
        transform.Translate(0, 0, z);

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb.AddForce(new Vector3(0, gameObject.transform.position.y + 50.0f) * jumpHeight);
                //Debug.Log("jumping");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }*/
}
