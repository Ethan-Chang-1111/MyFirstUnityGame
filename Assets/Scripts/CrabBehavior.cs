using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehavior : EnemyBehavior
{
    /*public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] private GameObject deathEffect = null;
    [SerializeField] private GameObject healthBar = null;*/
    [SerializeField] private float velocity = 0f;

    //random movement
    float run = 0f;
    float nextRun = 0f;
    float timer = 0f;
    int timeReset = 5;



    /*public float damage = 5f;
    public float kbX = 1000f;
    public float kbY = 300f;*/

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
    /*

    public void Hit(float damage){
        health -= damage;

        if(health >= 0){
            int index = 0;
            float percent = (health/maxHealth);
            if(percent >= 1.0){
                index = 0;//(infin-100%]
            }else if(percent >= .75){
                index = 1;//(100%-75%]
            }else if(percent >= .5){
                index = 2;//(75%-50%]
            }else if(percent >= .25){
                index = 3;//(50%-25%]
            }else if(percent > 0){
                index = 3;//(25%-0%)
            }else if(percent == 0){
                index = 4;//[0%]
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

    public void Flip(){
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f);
	}

    public Rigidbody2D getRB(){
        return rb;
    }

    public Vector3 getOnHit(){
        return new Vector3(kbX,kbY,damage);
    }*/
}
