using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMove : MonoBehaviour
{
    //player status variables
    [SerializeField] private CharacterController2D controller = null;
    [SerializeField] private Rigidbody2D rb = null;
    float moveSpeed = 20f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    float maxHealth = 100;
    public float health;
    
    [SerializeField] private GameObject respawnPoint = null;

    float timer;
    bool isInvinc = false;
    float invicTime = 5f;

    //firepoint manipulation variables
    [SerializeField] private GameObject firepoint = null;
    Vector2 initFirePointRel;
    float firePointOffsetCrouch = .13f;
    float firePointOffsetLookUpX = .125f;
    float firePointOffsetLookUpY = .17f;

    Vector2 crouchFirePosRel;
    Vector2 upFirePosRel;

    //Animation variable
    public Animator animator;
    public SpriteRenderer spriteRender;
    Color initColor;
    bool IsOpaque = true;

    //Communicate with UI
    [SerializeField] private GameObject HealthUI = null;
    [SerializeField] private GameObject TextUI = null;
    HealthUI healthUI;
    Text text;
    
    void Awake(){
        initColor = spriteRender.color;

        //create relative positions for firepoint
        initFirePointRel = transform.InverseTransformPoint(firepoint.transform.position);//world space into local space
        crouchFirePosRel = new Vector2(initFirePointRel.x, initFirePointRel.y-firePointOffsetCrouch);//shift local space for crouch Pos
        upFirePosRel = new Vector2(initFirePointRel.x-firePointOffsetLookUpX, initFirePointRel.y+firePointOffsetLookUpY);//shift local space for lookup Pos

        health = maxHealth;
        healthUI = HealthUI.GetComponent<HealthUI>();
        text = TextUI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {   
        horizontalMove = Input.GetAxisRaw("Horizontal")*moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        updateHealth();

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

        //while up is pressed, deactivate crouch
        //while crouch is pressed deactivate up

        if(Input.GetButtonDown("Crouch")){
            crouch = true;
            changeFirePointCrouch(true);    
        }else if(Input.GetButtonUp("Crouch")){
            crouch = false;
            changeFirePointCrouch(false);
        }else if(Input.GetButtonDown("LookUp")){
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
            if(health>0){
                rb.AddForce(kb);
                updateHealth();
                animator.SetBool("Damaged",true);
                animator.SetBool("InAir",true);
            }else{
                dieAndRespawn();
            }
            isInvinc = true;//can no longer take damage until set to false;
        }
    }

    void updateHealth(){
        float percent = (health/maxHealth);

        int index = 0;
        float absolIndex = percent*4;
        float rounder = (absolIndex*10)%10;
        index = rounder>=5?(int)(absolIndex+1):(int)absolIndex; 
        healthUI.index = index;
        
        text.text =  "Health: " + health.ToString() + "/" + maxHealth.ToString();
    }

    void playerBlink(){
        if(IsOpaque){
            Color newColor = new Color(initColor.r,initColor.g,initColor.b,0.2f);
            spriteRender.color = newColor;
        }else{
            spriteRender.color = initColor;
        }
        IsOpaque = !IsOpaque;
    }

    //try to respawn to nearest respawn platform. Call a global game object?
    void dieAndRespawn(){
        transform.position = new Vector2(respawnPoint.transform.position.x,respawnPoint.transform.position.y+1.8f);
        health = maxHealth;
        updateHealth();
    }

    public void powerUp(bool active, int type){
        moveSpeed = (type == 2)?active?(moveSpeed*2):(moveSpeed):moveSpeed;
        health = (type == 3)?active?(health+40):(health):health;
    }

    void changeFirePointCrouch(bool yes){
        if(yes){
            firepoint.transform.position = transform.TransformPoint(crouchFirePosRel);//change rel pos to worl pos and transform
        }else{
           firepoint.transform.position = transform.TransformPoint(initFirePointRel);//go to init rel pos        
        }
        
    }
    
    void changeFirePointLookUp(bool yes){
        if(yes){
            firepoint.transform.position = transform.TransformPoint(upFirePosRel);//change rel pos to worl pos and transform
        }else{
            firepoint.transform.position = transform.TransformPoint(initFirePointRel);//go to init rel pos     
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