using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : BulletBase
{
    [SerializeField] private GameObject explosion = null;
    // Start is called before the first frame update
    public override void Start()
    {
        lifespan = 3;
        damage = 100f;
        speed = speed/2;
        rb.velocity = transform.right * speed;
    }

    //what the bullet does when it hits an object
    public override void endEffect(string tag){
        //make a large circle collider trigger and damage everything in it

        GameObject blast = Instantiate(explosion,transform.position,transform.rotation);
        Destroy(blast, .34f);
        Destroy(gameObject);
    }
}
