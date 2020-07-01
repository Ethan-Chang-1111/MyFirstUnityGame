using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBehavior : EnemyBehavior
{
    [SerializeField] private float jumpVelocity = 0f;

    //random movement
    float run = -1f;
    float random = 1f;
    float timer = 0f;
    float timeReset = 10;

    void Start(){
        damage = 5;
        kbX = 2000;
        kbY = 0;
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

    public override void OnCollisionEnter2D(Collision2D collision){
        GameObject hitObject = collision.gameObject;
        PlatformEffector2D tilemap = hitObject.GetComponent<PlatformEffector2D>();
        if(tilemap != null){//returns true if hit tilemap
            animator.SetBool("InAir",false);
        }
        if(hitObject.name == "Water"){
            Hit(99999999f);
        }
    }
}