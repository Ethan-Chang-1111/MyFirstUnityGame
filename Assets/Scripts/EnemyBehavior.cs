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

        if(health > 0){
            int index = 0;
            float percent = (health/maxHealth);

            float N = percent*4;
            //round
            float I = (N*10)%10;
            index = I>=5?(int)(N+1):(int)N; 

            //Debug.Log("%: " + percent + " i: " + index);
            GameObject healthObj = Instantiate(healthBar,gameObject.transform.position,Quaternion.Euler(0, 0, 0));
            healthObj.GetComponent<DisplayHealth>().display(index);
            Destroy(healthObj,.25f);
        }else{
            GameObject deathAni = Instantiate(deathEffect,gameObject.transform.position,gameObject.transform.rotation);
            Destroy(deathAni,0.5f);
            Destroy(gameObject);
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

    public override void OnCollisionEnter2D(Collision2D collision){
        GameObject hitInfo = collision.gameObject;
        if(hitInfo.name == "Water"){
            Hit(99999999f);
        }
    }
}
