using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    public CharacterController2D controller;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public float moveSpeed = 0f;

    //trying to make collider reduce when crouching
    public CapsuleCollider2D collider;
    Vector2 initialSize;
    Vector2 initialOffset;

    public GameObject firepoint;
    float firePointOffset = .65f;

    //Animation
    public Animator animator;
    
    void Awake(){
        initialSize = collider.size;
        initialOffset = collider.offset;
        //Debug.Log("initial Collider Size y " + initialColliderSize);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal")*moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump")){
            jump = true;
            animator.SetBool("Jump",true);
        }
        if(Input.GetButtonDown("Crouch")){
            crouch = true;
            collider.size = new Vector2(collider.size.x,.15f);
            collider.offset = new Vector2(collider.offset.x,-.15f);
            firepoint.GetComponent<Transform>().position = new Vector2(firepoint.GetComponent<Transform>().position.x,firepoint.GetComponent<Transform>().position.y-firePointOffset);
        }else if(Input.GetButtonUp("Crouch")){
            crouch = false;
            collider.size = initialSize;
            collider.offset = initialOffset;
            firepoint.GetComponent<Transform>().position = new Vector2(firepoint.GetComponent<Transform>().position.x,firepoint.GetComponent<Transform>().position.y+firePointOffset);
        }
    }

    void FixedUpdate(){
        controller.Move(horizontalMove * Time.fixedDeltaTime,crouch,jump);
        jump = false;
        animator.SetBool("Jump", false);
        animator.SetBool("InAir", true);
    }

    //InAir is not getting set to false when it is grounded
    public void OnLanding(){
        animator.SetBool("InAir", false);
        //Debug.Log("Jump to false");
    }
    public void OnCrouching(bool isCrouching){
        animator.SetBool("Crouch", isCrouching);
        //firepointPos.position.y = firepointPos.position.y/2; 
        //Debug.Log("Crouch to " + isCrouching);
    }
}
