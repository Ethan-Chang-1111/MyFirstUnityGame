using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject deathEffect;
    public GameObject healthBar;
    public float jumpVelocity = 30f;
    bool facingRight = false;

    //random movement
    float run = -1f;
    float random = 1f;
    float timer = 0f;
    float timeReset = 10;
    
    float maxHealth = 100f;
    float health;

    public float damage = 10f;
    public float kbX = 2000f;
    public float kbY = 0f;

    void Start(){
        health = maxHealth;
        random = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.fixedDeltaTime;  
        if(((int) timer) == timeReset){
            run *= -1;
        } 
        random = Random.Range(0f, 1f);
    }

    void FixedUpdate(){
        Vector2 targetVelocity = rb.velocity;
        if(run == 1){
            float targetY = run*jumpVelocity;
            float targetX = 0f;
            if(random > .5f){
                targetX = jumpVelocity/2;
            }else{
                targetX = -1 * (jumpVelocity/2);
            }
            targetVelocity = new Vector2(rb.velocity.x + targetX, rb.velocity.y + targetY);
            rb.velocity = targetVelocity;
            run *= -1;
            timer = 0;
            animator.SetBool("InAir",true);
        }

        if(targetVelocity.x > 0 && !facingRight){//moving right, but facing left
			Flip();
		}else if (targetVelocity.x < 0 && facingRight){//moving left, but facing right
			Flip();
		}
    }

    public void Hit(float damage){
        health -= damage;

        if(health >= 0){
            int index = 0;
            float percent = (health/maxHealth);
            if(percent >= 1.0){
                index = 0;
            }else if(percent >= .75){
                index = 1;
            }else if(percent >= .5){
                index = 2;
            }else if(percent >= .25){
                index = 3;
            }else if(percent > 0){
                index = 3;
            }else if(percent == 0){
                index = 4;
            }
            //Debug.Log("%: " + percent + " i: " + index);
            GameObject healthObj = Instantiate(healthBar,gameObject.transform.position,Quaternion.Euler(0, 0, 0));
            healthObj.GetComponent<DisplayHealth>().display(index);
            Destroy(healthObj,.25f);
        }

        if(health <=0){
            GameObject deathAni = Instantiate(deathEffect,gameObject.transform.position,gameObject.transform.rotation);
            Destroy(gameObject);
            Destroy(deathAni,0.5f);
        }
    }

    void Flip(){
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f);
	}

    void OnCollisionEnter2D(Collision2D collision){
        GameObject hitObject = collision.gameObject;
        PlatformEffector2D tilemap = hitObject.GetComponent<PlatformEffector2D>();
        if(tilemap != null){//returns true if hit tilemap
            animator.SetBool("InAir",false);
        }
    }
}