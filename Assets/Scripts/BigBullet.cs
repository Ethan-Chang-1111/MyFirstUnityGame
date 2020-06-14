using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : BulletBase
{
    // Start is called before the first frame update
    public override void Start()
    {
        lifespan = 7;
        damage = 50f;
        rb.velocity = transform.right * speed;
    }

    //what the bullet does when it hits an object
    public override void endEffect(string tag){
        GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
        Destroy(bulletAnim, .34f);

        if(tag == "Ground" || tag == ""){//returns true if hit tilemap or timed out
            Destroy(gameObject);//only destroys by time or hit the tilemap
        }
    }
}