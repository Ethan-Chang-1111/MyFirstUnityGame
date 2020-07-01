using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoBehavior :  EnemyBehavior
{
    [SerializeField] private float velocity = 0f;

    //random movement
    float runY = 0f;
    float runX = 0f;
    float timer = 0f;
    float timeReset = 5f;

    void Start(){
        damage = 20;
        kbX = 300;
        kbY = 300;
        health = maxHealth;
        runX = Random.Range(-1f, 1f) * velocity;
        runY = Random.Range(-1f, 1f) * velocity;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.fixedDeltaTime;  
        if(((int) timer) == timeReset){
            runX = Random.Range(-1f, 1f) * velocity;
            runY = Random.Range(-1f, 1f) * velocity;
            timer = 0f;
        }
    }

    void FixedUpdate(){
        Vector3 targetVelocity = new Vector2(runX, runY);
        rb.velocity = targetVelocity;

        if(targetVelocity.x > 0 && !facingRight){//moving right, but facing left
			Flip();
		}else if (targetVelocity.x < 0 && facingRight){//moving left, but facing right
			Flip();
		}
    }
}