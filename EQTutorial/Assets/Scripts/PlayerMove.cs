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
    private GameObject Persona;
    private GameObject TargetHealth;
    private GameObject MyName;
    private GameObject SpellBook;
    public GameObject ChatBox;
    public GameObject ScrollArea;
    public InputField TextInput;
    private bool OpenChatBox = false;

    Ray ray;
    RaycastHit hit;

    void Start() {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        Persona = GameObject.Find("persona");
        TargetHealth = GameObject.Find("TargetHealth");
        MyName = GameObject.Find("MyName");
        SpellBook = GameObject.Find("SpellBook");
    }

    void Update()
    {
        float rotateSpeed = 1.5f;
        float speed = 14f;

        // check if player on ground
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player..
        if (!OpenChatBox)
        {
            if (Input.GetButtonDown("Jump") && (groundedPlayer || UnderWater))
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            if (UnderWater)
            {
                playerVelocity.y += Mathf.Sqrt(1.0f * gravityValue);
            }
            playerVelocity.y += gravityValue * Time.deltaTime;
        }


        // Rotate around y - axis, ignore on right click
        if (!Input.GetMouseButton(1) && !OpenChatBox) {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        }

        // Move forward / backward
        Vector3 forward = new Vector3(0, 0, 0);
        if (!OpenChatBox)
        {
            //if (UnderWater) {
            //    controller.Move(new Vector3(0, 0.2f, 0));
            //} else {
            controller.Move(playerVelocity * Time.deltaTime);
            //}
            
            if (ClimbingLadder && Input.GetAxis("Vertical") > 1)
            {
                //Debug.Log("climbing ladder");
                transform.position += new Vector3(0, 50, 0);
            }
            else
            {
                forward = transform.TransformDirection(Vector3.forward);
            }
        }

        // right click, move camera and allow strafing
        if (!OpenChatBox)
        {
            if (Input.GetMouseButton(1) /*|| UnderWater*/)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 moveDirectionForward = transform.forward * verticalInput * Time.deltaTime;
                Vector3 moveDirectionSide = transform.right * horizontalInput * Time.deltaTime;
                controller.SimpleMove((moveDirectionForward + moveDirectionSide).normalized * speed);
            }
            else
            {
                float curSpeed = speed * Input.GetAxis("Vertical");
                controller.SimpleMove(forward * curSpeed);
            }
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

                if (hit.collider.name == "WelcomeOkButton")
                {
                    GameObject wm = GameObject.Find("WelcomeMsg");
                    if (wm != null)
                        wm.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (OpenChatBox)
            {
                Debug.Log("closing OpenChatBox with text " + TextInput.text + " inside.");
                if (TextInput.text != "")
                {
                    ChatTextArea.text += "<color=black>You say '" + TextInput.text + "'</color>" + '\n';
                    ChatTextArea.rectTransform.sizeDelta = new Vector2(ChatTextArea.rectTransform.sizeDelta.x, ChatTextArea.rectTransform.sizeDelta.y + 30);
                }
                OpenChatBox = false;
            }
            else
                OpenChatBox = true;
        }
        if (OpenChatBox) { // open text input
            TextInput.interactable = true;
            TextInput.Select();
            TextInput.ActivateInputField();
            Vector3 MoveTo = new Vector3(ChatBox.transform.position.x, 32, ChatBox.transform.position.z);
            ChatBox.transform.position = Vector3.Lerp(ChatBox.transform.position, MoveTo, 0.3f);
            MoveTo = new Vector3(ScrollArea.transform.position.x, 147f, ScrollArea.transform.position.z);
            ScrollArea.transform.position = Vector3.Lerp(ScrollArea.transform.position, MoveTo, 0.3f);
            ScrollArea.transform.localScale = new Vector3(1,0.75f,1);
        } 
        else // close text input
        {
            TextInput.text = "";
            TextInput.interactable = false;
            Vector3 MoveTo = new Vector3(ChatBox.transform.position.x, -32, ChatBox.transform.position.z);
            ChatBox.transform.position = Vector3.Lerp(ChatBox.transform.position, MoveTo, 0.3f);
            MoveTo = new Vector3(ScrollArea.transform.position.x, 125f, ScrollArea.transform.position.z);
            ScrollArea.transform.position = Vector3.Lerp(ScrollArea.transform.position, MoveTo, 0.3f);
            ScrollArea.transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.H) && !OpenChatBox)
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

        if (Input.GetKeyDown(KeyCode.I) && !OpenChatBox)
        {
            if (SpellBook.activeSelf)
            {
                InventoryMenu();
            }
            else
            {
                InventoryMenu(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !OpenChatBox)
        {
            Application.Quit();
        }
    }

    private void InventoryMenu(bool open = true)
    {
        if (open)
        {
            GameObject.Find("MainUI").GetComponent<RawImage>().texture = Resources.Load<Texture2D>("main_ui_2");
            Persona.SetActive(false);
            TargetHealth.SetActive(false);
            TargetName.gameObject.SetActive(false);
            MyName.SetActive(false);
            SpellBook.SetActive(false);
        }
        else
        {
            GameObject.Find("MainUI").GetComponent<RawImage>().texture = Resources.Load<Texture2D>("main_ui");
            Persona.SetActive(true);
            TargetHealth.SetActive(true);
            TargetName.gameObject.SetActive(true);
            MyName.SetActive(true);
            SpellBook.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "ladder") {
            ClimbingLadder = true;
            Debug.Log("on ladder");
        }
        if (collision.gameObject.name == "Exit")
        {
            Application.Quit();
            Debug.Log("quitting");
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
