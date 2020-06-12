using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : BulletAbstract
{
    public Rigidbody2D rb = null;
    public GameObject impactEffect = null;
    public float damage = 10f;
    public float speed = 10f;
    public int lifespan = 1;
    public float timeExisting = 0f;

    public override void Start(){
        rb.velocity = transform.right * speed;
    }
    
    public override void FixedUpdate(){
        timeExisting += Time.fixedDeltaTime;  
        if(timeExisting >= lifespan){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            Destroy(gameObject);
            Destroy(bulletAnim, .34f);
        } 
    }

    public override void OnTriggerEnter2D(Collider2D hitInfo){
        if(hitInfo.CompareTag("Enemy") || hitInfo.CompareTag("Ground")){//true if hit enemy or ground
            EnemyParent hitObject = hitInfo.GetComponent<EnemyParent>();
            if(hitObject != null){
                hitObject.Hit(damage);
            }
            endEffect(hitInfo.tag);
        }
    }    

    public override void endEffect(string tag){
        GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
        Destroy(gameObject);
        Destroy(bulletAnim, .34f);
    }
}
