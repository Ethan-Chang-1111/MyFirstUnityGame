using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    //player status variables
    [SerializeField] private CharacterController2D controller = null;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private float moveSpeed = 0f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    [SerializeField] private float maxHealth = 100;
    float health;

    float timer;
    bool isInvinc = false;
    float invicTime = 5f;

    //firepoint manipulation variables
    [SerializeField] private GameObject firepoint = null;
    Vector2 initFirePointRelPos;
    float firePointOffsetCrouch = .14f;
    float firePointOffsetLookUpX = .125f;
    float firePointOffsetLookUpY = .17f;

    //Animation variable
    [SerializeField] private Animator animator = null;
    [SerializeField] private SpriteRenderer spriteRender = null;
    Color initColor;
    bool IsOpaque = true;
    
    void Awake(){
        health = maxHealth;
        initColor = spriteRender.color;

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
            Vector2 KB = calcKB(AB,hitObject.getOnHit().x,hitObject.getOnHit().y);
            takeDamage(hitObject.getOnHit().z, KB);
        }
    }

    Vector2 calcKB(Vector2 enemyPos, float kbX, float kbY){
        Vector2 noMag = enemyPos.normalized;//Remove magnitude and keep direction of AB
        return new Vector2(-(noMag.x*kbX),-(noMag.y*kbY));//increase magnitude of AB to right kb value and reverse direction
    }
    
    void takeDamage(float dmg, Vector2 kb){
        if(!isInvinc){//if not invincible
            health -= dmg;
            animator.SetBool("Damaged",true);
            animator.SetBool("InAir",true);
            isInvinc = true;//can no longer take damage until set to false;
            rb.AddForce(kb);
            Debug.Log(health);
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

    public void heal(float amount){
        health += amount;
        Debug.Log("Called");
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