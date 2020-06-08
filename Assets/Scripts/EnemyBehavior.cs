using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float velocity = 30f;
    public Rigidbody2D m_Rigidbody2D;
    float run = 0f;
    bool facingRight = false;

    float timeDelay = 2f;
    int hits = 0;

    void Start(){
        run = Random.Range(-1f, 1f) * velocity;
    }
    // Update is called once per frame
    void Update()
    {
        timeDelay = (timeDelay + 1) % 1000;
        if(timeDelay == 1){
            run = run * -1;
        } 
        

    }
    void FixedUpdate(){
        Vector3 targetVelocity = new Vector2(run, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = targetVelocity;

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
        
        //Instantiate(gameObject,gameObject.GetComponent<Transform>().position,gameObject.GetComponent<Transform>().rotation);
        Destroy(gameObject);
    }
    void Flip(){
		facingRight = !facingRight;

		transform.Rotate(0f, 180f, 0f);
	}
}
