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

    //firepoint maniuplations
    public GameObject firepoint;
    float firePointOffsetCrouch = .13f;
    float firePointOffsetLookUpX = .125f;
    float firePointOffsetLookUpY = .17f;
    Vector2 initFirePointRelPos;

    //Animation
    public Animator animator;
    
    void Awake(){
        initFirePointRelPos = transform.InverseTransformPoint(firepoint.transform.position);//world space into local space
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
        if(yes){
            Vector2 relTransform = (new Vector2(initFirePointRelPos.x, initFirePointRelPos.y-firePointOffsetCrouch));//transform the relative position
            firepoint.transform.position = transform.TransformPoint(relTransform);//change rel pos to worl pos and transform
        }else{
            firepoint.transform.position = transform.TransformPoint(initFirePointRelPos);//go to init rel pos        
        }
    }
    
    void changeFirePointLookUp(bool yes){
        if(yes){
            Vector2 relTransform = (new Vector2(initFirePointRelPos.x-firePointOffsetLookUpX, initFirePointRelPos.y+firePointOffsetLookUpY));//transform the relative position
            firepoint.transform.position = transform.TransformPoint(relTransform);//change rel pos to worl pos and transform
        }else{
            firepoint.transform.position = transform.TransformPoint(initFirePointRelPos);//go to init rel pos     
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
