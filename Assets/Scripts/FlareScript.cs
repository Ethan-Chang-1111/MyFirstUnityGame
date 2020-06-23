using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareScript : BulletBase
{
    
    public override void Start(){
        timeExisting = 0f;
        lifespan = 20;
    }

    public override void OnTriggerEnter2D(Collider2D collider){
        //do nothing
    }

    void OnCollisionEnter2D(Collision2D collision){
        GameObject hitInfo = collision.gameObject;
        if(hitInfo.name == "Player" || hitInfo.name == "Flare(Clone)" || hitInfo.CompareTag("Enemy")){
            //Debug.Log(hitInfo.name);
        BasicMove player = hitInfo.GetComponent<BasicMove>();
            Physics2D.IgnoreCollision(hitInfo.GetComponent<Collider2D>(),GetComponent<Collider2D>(), true);
        }
        if(hitInfo.name == "Water"){
            endEffect("");
        }
    }
}
