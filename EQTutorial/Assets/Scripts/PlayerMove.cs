using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float jumpHeight = 4.0f;
    private float gravityValue = -9.81f;
    CharacterController controller;
    public bool UnderWater = false;
    public bool ClimbingLadder = false;
    public Text TargetName;
    public Text ChatTextArea;
    Rigidbody rb;
    public GameObject OkMsgBox;

    Ray ray;
    RaycastHit hit;

    void Start() {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float rotateSpeed = 3f;
        float speed = 14f;

        // check if player on ground
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

        // Rotate around y - axis, ignore on right click
        if (!Input.GetMouseButton(1)) {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        }

        //if (UnderWater) {
        //    controller.Move(new Vector3(0, 0.2f, 0));
        //} else {
            controller.Move(playerVelocity * Time.deltaTime);
        //}

        // Move forward / backward
        Vector3 forward = new Vector3(0,0,0);
        if (ClimbingLadder && Input.GetAxis("Vertical") > 1) {
            //Debug.Log("climbing ladder");
            transform.position += new Vector3(0, 50, 0);
        } else {
            forward = transform.TransformDirection(Vector3.forward);
        }

        // right click, move camera and allow strafing
            if (Input.GetMouseButton(1) /*|| UnderWater*/) {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 moveDirectionForward = transform.forward * verticalInput * Time.deltaTime;
                Vector3 moveDirectionSide = transform.right * horizontalInput * Time.deltaTime;
                controller.SimpleMove((moveDirectionForward + moveDirectionSide).normalized * speed);
            } else {
                float curSpeed = speed * Input.GetAxis("Vertical");
                controller.SimpleMove(forward * curSpeed);
            }

        if (Input.GetMouseButtonDown(0)){
            // left click on NPC, change UI target name
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "NPC") {
                    TargetName.text = hit.collider.GetComponent<NPC>().NPCName;
                    if (TargetName.text.Equals("Prumpy Irontoe"))
                    {
                        // add text to text area
                        ChatTextArea.text += '\n' + "<color=white>Prumpy Irontoe says, 'Good! Now press the H key.'</color>" + '\n';

                        // expand text box
                        ChatTextArea.rectTransform.sizeDelta = new Vector2(ChatTextArea.rectTransform.sizeDelta.x, ChatTextArea.rectTransform.sizeDelta.y + 40);
                        
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatTextArea.text += "<color=white>Hail, " + TargetName.text + "</color>" + '\n';
            ChatTextArea.rectTransform.sizeDelta = new Vector2(ChatTextArea.rectTransform.sizeDelta.x, ChatTextArea.rectTransform.sizeDelta.y + 20);

            if (TargetName.text.Equals("Prumpy Irontoe"))
            {
                // add text to text area
                ChatTextArea.text += "<color=white>Prumpy Irontoe says, 'Greetings Soandso! Very good. That is the first step to communicating with the many people and [creatures] you will meet on Norrath.'</color>" + '\n';

                // expand text box
                ChatTextArea.rectTransform.sizeDelta = new Vector2(ChatTextArea.rectTransform.sizeDelta.x, ChatTextArea.rectTransform.sizeDelta.y + 50);

                OkMsgBox.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "ladder") {
            ClimbingLadder = true;
            Debug.Log("on ladder");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "ladder") {
            ClimbingLadder = false;
            Debug.Log("off ladder");
        }
    }
}
