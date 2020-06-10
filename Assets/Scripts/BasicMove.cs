using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    //player status variables
    public CharacterController2D controller;
    public Rigidbody2D rb;
    public float moveSpeed = 0f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    float maxHealth = 100;
    float health;

    float timer;
    bool isInvinc = false;
    float invicTime = 2f;
    float kbX=200;
    float kbY=200;

    //firepoint manipulation variables
    public GameObject firepoint;
    Vector2 initFirePointRelPos;
    float firePointOffsetCrouch = .13f;
    float firePointOffsetLookUpX = .125f;
    float firePointOffsetLookUpY = .17f;

    //collider manipulation variables
    public CapsuleCollider2D collider;
    Vector2 initialSize;
    Vector2 initialOffset;

    //Animation variable
    public Animator animator;
    
    void Awake(){
        health = maxHealth;

        initialSize = collider.size;
        initialOffset = collider.offset;

        initFirePointRelPos = transform.InverseTransformPoint(firepoint.transform.position);//world space into local space
    }

    // Update is called once per frame
    void Update()
    {   
        horizontalMove = Input.GetAxisRaw("Horizontal")*moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if(isInvinc){
            timer += Time.fixedDeltaTime;
            if(timer >= invicTime){
                animator.SetBool("Damaged",false);
               isInvinc = false;
               timer = 0;
            }
        }

        if(Input.GetButtonDown("Jump")){
            jump = true;
            animator.SetBool("Jump",true);
        }

        if(Input.GetButtonDown("Crouch")){
            crouch = true;
            collider.size = new Vector2(collider.size.x,.15f);
            collider.offset = new Vector2(collider.offset.x,-.15f);
            changeFirePointCrouch(true);    
        }else if(Input.GetButtonUp("Crouch")){
            crouch = false;
            collider.size = initialSize;
            collider.offset = initialOffset;
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

    void OnCollisionEnter2D(Collision2D collision){
            GameObject hitInfo = collision.gameObject;
            EnemyBehavior hitObject1 = hitInfo.GetComponent<EnemyBehavior>();
            JumperBehavior hitObject2 = hitInfo.GetComponent<JumperBehavior>();
            OctoBehavior hitObject3 = hitInfo.GetComponent<OctoBehavior>();
            if(hitObject1 != null){//true if hit an enemy
                takeDamage(1);
            }else if(hitObject2 != null){
                takeDamage(1);
            }else if (hitObject3 != null){
                takeDamage(1);
            }
        
    }

    void takeDamage(float dmg){
        if(!isInvinc){//if not invincible
            health -= dmg;
            animator.SetBool("Damaged",true);
            isInvinc = true;//can no longer take damage until set to false;
            rb.AddForce(new Vector2(kbX, kbY));
            Debug.Log(health);
        }

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

    //When there is an object above the player and the player jumps, the player never un-detects the ground
    //the player jumps and lands immediately, so OnLanding is not called
    public void OnLanding(){
        animator.SetBool("InAir", false);
    }

    public void OnCrouching(bool isCrouching){
        animator.SetBool("Crouch", isCrouching);
    }
}