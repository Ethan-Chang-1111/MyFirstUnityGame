using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : EnemyParent
{
    //defualt behavior for an Enemy
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject deathEffect = null;
    public GameObject healthBar = null;

    public bool facingRight = false;
    public float maxHealth = 100f;
    public float health;

    public float damage = 5f;
    public float kbX = 1000f;
    public float kbY = 300f;

    public override void Hit(float damage){
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

    public override void Flip(){
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f);
	}

    public override Rigidbody2D getRB(){
        return rb;
    }

    public override Vector3 getOnHit(){
        return new Vector3(kbX,kbY,damage);
    }
}