using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 100;
    public float walkSpeed = 0.1f;
    public float runSpeed = 0.05f;
    public float jumpHeight = 1;
    public float groundCheckRadius = 0.1f;
    public bool grounded = false;
    public LayerMask layer;
    Vector3 velocity;
    public Transform groundCheck;
    Rigidbody rb;
    CharacterController controller;

    Inventory inventory;
    float mouseX;
    float mouseY;
    Vector3 playerScale;
    void Start()
    {
        playerScale = transform.localScale;
        //Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<Inventory>();
        inventory.ShowInventory();

    }

    
    void Update()
    {
        bool press = Input.GetKeyDown(KeyCode.E);
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.ShowInventory();

        }
        
        if (GameManager._GameManager.freeze)
        {
            return;
        }
        HandleCameraRotation();
        HandlePlayerMovment();

        if (Input.GetMouseButtonDown(0) && GameManager._GameManager.freeze == false && Player._Player.hand.childCount !=0)
        {
            Player._Player.UseItemInHand();
        }
        //Ground check using sphere
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius,layer);

        if (grounded)
        {
            
            velocity.y = 0;
        }
           
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
    void HandleCameraRotation()
    {

        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -90, 90);


        Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(-mouseY, 0, 0));
        transform.localRotation = Quaternion.Euler(new Vector3(0, mouseX, 0));
    }
    //Energy balance
    void HandlePlayerMovment()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOver._GameOver.OnGameOver("Random thing");
        }
        float ver = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        float acceleration = 0;
        float energy = Player._Player.energy;
        float stamina = Player._Player.stamina;
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && energy > 0 && ver !=0)
        {
            acceleration = runSpeed;
            stamina -= Time.deltaTime*0.2f;
            energy -= Time.deltaTime * 0.01f;

            

        }
        else
        {
            acceleration = walkSpeed;
            if (Input.GetKey(KeyCode.LeftShift) == false && stamina < 1)
            {
                stamina += Time.deltaTime*0.15f;
                
            }
            if (ver != 0)
                energy -= Time.deltaTime * 0.005f;

            

        }
        //Mathf clamp rounds up
        Player._Player.energy = Mathf.Clamp01(energy);
        Player._Player.stamina = Mathf.Clamp(stamina, 0, Player._Player.maxStamina);
        Player._Player.onPlayerStateChanged?.Invoke();

        if (energy < 0.15)
        {
            acceleration *= 0.5f;
            
        }
        
        if (energy == 0 && (ver != 0 || hor !=0 )&& Random.Range(0,10) <3)
        {
            StartCoroutine(Player._Player.sleep((1 - energy) * 8));
        }


        Vector3 move = (transform.forward * ver + transform.right * hor) * acceleration * Time.deltaTime*(Player._Player.underwater == true? 0.5f :1);
        velocity.y += 0.5f * -9.8f * Time.deltaTime * Time.deltaTime;
        
        if(grounded && Input.GetKey(KeyCode.Space) && Player._Player.energy > 0.1f)
        {
            move += transform.up * jumpHeight;
        }
       

        controller.Move(move);
        controller.Move(velocity);
    }

   

}
