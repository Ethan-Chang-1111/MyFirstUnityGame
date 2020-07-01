using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehavior : EnemyBehavior
{
    [SerializeField] private float velocity = 0f;

    //random movement
    float run = 0f;
    float nextRun = 0f;
    float timer = 0f;
    int timeReset = 5;

    void Start(){
        damage = 10;
        kbX = 1000;
        kbY = 300;
        health = maxHealth;
        run = Random.Range(-1f, 1f) * velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //when timer equals time reset, object moves
        //once timer goes above time reset, object stops, randomizes the next movement, and resets timer
        timer += Time.fixedDeltaTime;  
        if(((int) timer) == timeReset){
            run = nextRun;
        }else if(((int) timer) > timeReset) {
            run = 0f;
            nextRun = Random.Range(-1f, 1f) * velocity;
            timer = 0f;
        } 
    }

    void FixedUpdate(){
        Vector3 targetVelocity = new Vector2(run, rb.velocity.y);
        rb.velocity = targetVelocity;
        animator.SetFloat("Speed", Mathf.Abs(run));

        if(targetVelocity.x > 0 && !facingRight){//moving right, but facing left
			Flip();
		}else if (targetVelocity.x < 0 && facingRight){//moving left, but facing right
			Flip();
		}
    }
}
