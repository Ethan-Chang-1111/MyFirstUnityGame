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
    float firePointOffsetCrouch = .65f;
    float firePointOffsetLookUpX = .275f;
    float firePointOffsetLookUpY = .575f;

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
            changeFirePointCrouch(true);    
        }else if(Input.GetButtonUp("Crouch")){
            crouch = false;
            changeFirePointCrouch(false);
        }

        if(Input.GetButtonDown("LookUp")){
            changeFirePointLookUp(true);
            animator.SetBool("AimUp",true);
        }else if(Input.GetButtonUp("LookUp")){
            changeFirePointLookUp(false);
            animator.SetBool("AimUp",false);
        }

        
    }

    void FixedUpdate(){
        if(jump){
            animator.SetBool("InAir", true);
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime,crouch,jump);
        jump = false;
        animator.SetBool("Jump", false);
    }

    void changeFirePointCrouch(bool yes){
        Vector2 currentPos = firepoint.GetComponent<Transform>().position;
        if(yes){
            collider.size = new Vector2(collider.size.x,.15f);
            collider.offset = new Vector2(collider.offset.x,-.15f);
            firepoint.GetComponent<Transform>().position = new Vector2(currentPos.x, currentPos.y-firePointOffsetCrouch);
        }else{
            collider.size = initialSize;
            collider.offset = initialOffset;
            firepoint.GetComponent<Transform>().position = new Vector2(currentPos.x, currentPos.y+firePointOffsetCrouch);
        
        }
    }
    
    void changeFirePointLookUp(bool yes){
        Vector2 currentPos = firepoint.GetComponent<Transform>().position;
        if(yes){
            firepoint.GetComponent<Transform>().position = new Vector2(currentPos.x-firePointOffsetLookUpX, currentPos.y+firePointOffsetLookUpY);
        }else{
            firepoint.GetComponent<Transform>().position = new Vector2(currentPos.x+firePointOffsetLookUpX, currentPos.y-firePointOffsetLookUpY);
        }
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
