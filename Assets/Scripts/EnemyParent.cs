using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyParent: MonoBehaviour
{
    //MonoBehaviour is the parent class for all objects
    //use this as a parent interface for all enemies

    public abstract void Hit(float damage);
    public abstract void Flip();

    public abstract Rigidbody2D getRB();

    //use Vector3 to return 3 floats
    //kbX, kbY, damage
    public abstract Vector3 getOnHit();
    public abstract void OnCollisionEnter2D(Collision2D collision);


}
