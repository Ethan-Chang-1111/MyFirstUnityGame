using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMove : MonoBehaviour
{
    //player status variables
    [SerializeField] private CharacterController2D controller = null;
    [SerializeField] private Rigidbody2D rb = null;
    float moveSpeed = 0f;
    float baseMoveSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    GameObject respawnPoint;
    float maxHealth = 100;
    public float health;
    int lives = 10;
    float timer;
    bool isInvinc = false;
    float invicTime = 5f;

    //firepoint manipulation variables
    [SerializeField] private GameObject firepoint = null;
    Vector2 initFirePointRel;
    Quaternion initFirePointRot;
    Quaternion firePointLookUpRot;
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
    [SerializeField] private GameObject LivesUI = null;
    HealthUI healthUI;
    Text healthText;
    Text livesText;

    [SerializeField] private GameMaster gm = null;
    
    void Awake(){
        initColor = spriteRender.color;
        moveSpeed = baseMoveSpeed;

        //create relative positions for firepoint
        initFirePointRel = transform.InverseTransformPoint(firepoint.transform.position);//world space into local space
        crouchFirePosRel = new Vector2(initFirePointRel.x, initFirePointRel.y-firePointOffsetCrouch);//shift local space for crouch Pos
        upFirePosRel = new Vector2(initFirePointRel.x-firePointOffsetLookUpX, initFirePointRel.y+firePointOffsetLookUpY);//shift local space for lookup Pos

        initFirePointRot = transform.rotation;//get initial rotation
        firePointLookUpRot = Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z+90f);

        health = maxHealth;
        healthUI = HealthUI.GetComponent<HealthUI>();
        healthText = TextUI.GetComponent<Text>();
        livesText = LivesUI.GetComponent<Text>();
        updateLives();
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
            changeFirePointCrouch(true);    
            crouch = true;
        }else if(Input.GetButtonUp("Crouch")){
            changeFirePointCrouch(false);
            crouch = false;
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

        if(hitInfo.name == "Water"){
            dieAndRespawn();
        }
    }

    void OnTriggerEnter2D(Collider2D obj){
        GameObject hitInfo = obj.gameObject;
        if(hitInfo.CompareTag("RespawnPlatform")){
            //Debug.Log(hitInfo.name);
            hitInfo.GetComponent<RespawnPlatform>().turnOn();
            if(respawnPoint != null && respawnPoint != hitInfo){//turn previous one off
                respawnPoint.GetComponent<RespawnPlatform>().turnOff();
            }
            respawnPoint = hitInfo;
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
        
        healthText.text =  "Health: " + health.ToString() + "/" + maxHealth.ToString();
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

    void updateLives(){
        if(lives<=0){
            Debug.Log("Dead");
        }
        livesText.text = "Lives: " + lives;
    }

    void dieAndRespawn(){
        for(int i = 0; i<10;i++){
            powerUp(false,i);
            gameObject.GetComponent<playerWeapon>().powerUp(false,i);
        }
        for(int i = 0; i<10; i++){
            gameObject.GetComponent<playerWeapon>().setAmmo(100);
        }
        lives -= 1;
        updateLives();
        health = maxHealth;
        updateHealth();
        transform.position = new Vector2(respawnPoint.transform.position.x,respawnPoint.transform.position.y+1.8f);

        gm.reloadScene();
    }

    public void powerUp(bool active, int type){
        moveSpeed = (type == 2)?active?(baseMoveSpeed*2):(baseMoveSpeed):moveSpeed;
        health = (type == 3)?active?(health+40):(health):health;
    }

    void changeFirePointCrouch(bool yes){
        if(yes){
            animator.SetBool("Crouch", yes);
            firepoint.transform.position = transform.TransformPoint(crouchFirePosRel);//change rel pos to worl pos and transform
        }else{
            animator.SetBool("Crouch", yes);    
            firepoint.transform.position = transform.TransformPoint(initFirePointRel);//go to init rel pos  
        }
        
    }
    
    void changeFirePointLookUp(bool yes){
        if(yes){
            firepoint.transform.position = transform.TransformPoint(upFirePosRel);//change rel pos to worl pos and transform
            firepoint.transform.rotation = firePointLookUpRot;

        }else{
            firepoint.transform.position = transform.TransformPoint(initFirePointRel);//go to init rel pos  
            firepoint.transform.rotation = firepoint.transform.parent.transform.rotation; 
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