using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float velocity = 30f;
    public Rigidbody2D rb;
    float run = 0f;
    float nextRun = 0f;
    bool facingRight = false;

    float timer = 0f;
    int timeReset = 5;
    int hits = 0;

    public Animator animator;
    public GameObject deathEffect;

    void Start(){
        run = Random.Range(-1f, 1f) * velocity;
    }
    // Update is called once per frame
    void Update()
    {
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

    public void Hit(){
        //Debug.Log("Hit registered");
        //hits++;
        //Debug.Log(hits);
        
        GameObject deathAni = Instantiate(deathEffect,gameObject.GetComponent<Transform>().position,gameObject.GetComponent<Transform>().rotation);
        Destroy(gameObject);
        Destroy(deathAni,0.5f);
    }
    void Flip(){
		facingRight = !facingRight;

		transform.Rotate(0f, 180f, 0f);
	}
}
