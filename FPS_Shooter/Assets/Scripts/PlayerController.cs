using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController controller;


    private Vector3 moveInput;

    public Transform camTrans;

    public bool invertX;
    public bool invertY;
    public float mouseSensitivity;

    private bool canJump,canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask GroundName;


    public Animator anim;

    public GameObject bullet;
    public Transform GunPoint;
    void Update()
    {
        //player movement logic
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //store y velocity
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
        moveInput = vertMove + horiMove;
        moveInput.Normalize();
       

        //Running
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime; 

       

        if(controller.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        if(canJump)
        {
            canDoubleJump = true;
        }


        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, GroundName).Length > 0;

        //jumping 
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        }

       else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
       {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }

        controller.Move(moveInput * Time.deltaTime);

        //mouse rotation logic
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y
            + mouseInput.x, transform.rotation.eulerAngles.z);

        if(invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if(invertY)
        {
            mouseInput.y = -mouseInput.y;
        }


        //handle shooting
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, GunPoint.position, GunPoint.rotation);
        }


        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(mouseInput.y, 0f, 0f));
        anim.SetFloat("Moving", moveInput.magnitude);
    }
}
