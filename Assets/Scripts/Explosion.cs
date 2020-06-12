using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BulletBase
{
    public override void Start(){
        lifespan = 1;
        damage = 100f;
        speed = 0;
        rb.velocity = transform.right * speed;
    }

}
