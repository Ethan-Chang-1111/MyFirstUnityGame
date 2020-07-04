using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoBehavior :  EnemyBehavior
{
    /*public Rigidbody2D rb;
    [SerializeField] private GameObject deathEffect = null;
    [SerializeField] private GameObject healthBar = null;*/
    [SerializeField] private float velocity = 0f;
    //bool facingRight = false;

    //random movement
    float runY = 0f;
    float runX = 0f;
    float timer = 0f;
    float timeReset = 5f;

    /*float maxHealth = 100f;
    float health;
    
    public float damage = 20f;
    public float kbX = 300f;
    public float kbY = 300f;*/

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
    /*
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