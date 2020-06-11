using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAbstract : MonoBehaviour
{
    public abstract void Start();
    
    public abstract void FixedUpdate();

    public abstract void OnTriggerEnter2D(Collider2D hitInfo);    

    public abstract void endEffect(string tag);
}
