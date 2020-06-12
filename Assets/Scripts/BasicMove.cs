using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    //player status variables
    [SerializeField] private CharacterController2D controller = null;
    [SerializeField] private Rigidbody2D rb = null;
    float moveSpeed = 30f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    float maxHealth = 100;
    float health;

    float timer;
    bool isInvinc = false;
    float invicTime = 5f;

    //firepoint manipulation variables
    [SerializeField] private GameObject firepoint = null;
    Vector2 initFirePointRelPos;
    float firePointOffsetCrouch = .13f;
    float firePointOffsetLookUpX = .125f;
    float firePointOffsetLookUpY = .17f;

    //Animation variable
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRender = null;
    Color initColor;
    bool IsOpaque = true;

    //Communicate with UI
    [SerializeField] private GameObject UI = null;
    HealthUI healthUI;
    
    void Awake(){
        health = maxHealth;
        initColor = spriteRender.color;

        initFirePointRelPos = transform.InverseTransformPoint(firepoint.transform.position);//world space into local space

        healthUI = UI.GetComponent<HealthUI>();
    }

    // Update is called once per frame
    void Update()
    {   
        horizontalMove = Input.GetAxisRaw("Horizontal")*moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(isInvinc){
            timer += Time.fixedDeltaTime;
            if(timer >= invicTime){
               isInvinc = false;
               timer = 0;
               animator.SetBool("InAir",false);
               animator.SetBool("Damaged",false);
            }else{
                playerBlink();
            }
        }

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

    void OnCollisionEnter2D(Collision2D collision){
        GameObject hitInfo = collision.gameObject;
        EnemyParent hitObject = hitInfo.GetComponent<EnemyParent>();
        if(hitObject != null){
            Vector2 AB = (hitObject.getRB().position - rb.position);//vector from player to obj
            Vector2 noMag = AB.normalized;//Remove magnitude and keep direction of AB
            Vector2 KB = new Vector2(-(noMag.x*hitObject.getOnHit().x),-(noMag.y*hitObject.getOnHit().y));//increase magnitude of AB to right kb value and reverse direction
            takeDamage(hitObject.getOnHit().z, KB);
        }
    }

    void takeDamage(float dmg, Vector2 kb){
        if(!isInvinc){//if not invincible
            health -= dmg;
            rb.AddForce(kb);
            Debug.Log(health);
            
            int index = 0;
            float percent = (health/maxHealth);
            float absolIndex = percent*4;
            float rounder = (absolIndex*10)%10;
            index = rounder>=5?(int)(absolIndex+1):(int)absolIndex; 

            healthUI.index = index;

            animator.SetBool("Damaged",true);
            animator.SetBool("InAir",true);
            isInvinc = true;//can no longer take damage until set to false;
        }
    }

    void playerBlink(){
        if(IsOpaque){
            Color newColor = new Color(initColor.r,initColor.g,initColor.b,0.5f);
            spriteRender.color = newColor;
        }else{
            spriteRender.color = initColor;
        }
        IsOpaque = !IsOpaque;
    }

    public void powerUp(bool active, int type){
        if(type == 1){
            moveSpeed = active?60f:30f;
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
        animator.SetBool("Damaged",false);
    }

    public void OnCrouching(bool isCrouching){
        animator.SetBool("Crouch", isCrouching);
    }
}