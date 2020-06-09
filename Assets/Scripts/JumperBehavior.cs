using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperBehavior : MonoBehaviour
{
    public float jumpVelocity = 30f;
    public Rigidbody2D rb;
    float run = -1f;
    float random = 1f;
    bool facingRight = false;

    float timer = 0f;
    float timeReset = 10;
    //int hits = 0;

    public Animator animator;
    public GameObject deathEffect;

    void Start(){
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

    public void Hit(){
        GameObject deathAni = Instantiate(deathEffect,gameObject.GetComponent<Transform>().position,gameObject.GetComponent<Transform>().rotation);
        Destroy(gameObject);
        Destroy(deathAni,0.5f);
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
