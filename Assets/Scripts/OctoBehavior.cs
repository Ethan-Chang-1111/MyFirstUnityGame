using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoBehavior : MonoBehaviour
{
    public float velocity = 30f;
    public Rigidbody2D rb;
    float runY = 0f;
    float runX = 0f;
    bool facingRight = false;
    bool facingUp = true;

    float timer = 0f;
    float timeReset = 5f;
    int hits = 0;

    void Start(){
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
			FlipX();
		}else if (targetVelocity.x < 0 && facingRight){//moving left, but facing right
			FlipX();
		}
    }

    public void Hit(){
        //Debug.Log("Hit registered");
        //hits++;
        //Debug.Log(hits);
        
        //Instantiate(gameObject,gameObject.GetComponent<Transform>().position,gameObject.GetComponent<Transform>().rotation);
        Destroy(gameObject);
    }

    void FlipX(){
		facingRight = !facingRight;

		transform.Rotate(0f, 180f, 0f);
	}
}
