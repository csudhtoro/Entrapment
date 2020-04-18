using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    public float moveSpeed;
    //public Rigidbody theRB;
    public CharacterController controller;
    public float jumpForce;

    private Vector3 moveDirection;
    public float gravityScale;
    //public Animator anim;


    // Start is called before the first frame update
    void Start() {
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {

        //using the rigidbody to control the player physics
        /*theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed , theRB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
        if(Input.GetButtonDown("Jump")) {
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
        }*/

        //
        float yStore = moveDirection.y;

        //allows player to move in the direction its facing, or go left and right, regardless of the camera position
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));

        //normalzing moveSpeed
        moveDirection = moveDirection.normalized * moveSpeed;

        moveDirection.y = yStore;

        if(controller.isGrounded) {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump")) {
                moveDirection.y = jumpForce;
            }
        }
        
        moveDirection.y +=  (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        //setting the animation
        //anim.SetBool("isGrounded", controller.isGrounded);
    }
}
