using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 0f;
    int timeToExist;
    float timeExisting = 0f;
    public GameObject impactEffect;
    public int shotVariant;
    // Start is called before the first frame update
    void Start()
    {
     rb.velocity = transform.right * speed;

     if(shotVariant == 1){
         timeToExist = 1;
     }else if(shotVariant == 2){
         timeToExist = 7;
     }
     
    }

    void FixedUpdate()
    {
        timeExisting += Time.fixedDeltaTime;  
        if(((int) timeExisting) == timeToExist){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            Destroy(gameObject);
            Destroy(bulletAnim, .34f);
        } 
    }

    //called when enter trigger
    void OnTriggerEnter2D(Collider2D hitInfo){
        //Debug.Log(hitInfo.name);

        BasicMove playerObject = hitInfo.GetComponent<BasicMove>();
        ShotScript shotObject = hitInfo.GetComponent<ShotScript>();

        //only destroys itself when it hits something other than a player or bullet
        if(playerObject == null && shotObject == null){
            EnemyBehavior hitObject = hitInfo.GetComponent<EnemyBehavior>();
            if(hitObject != null){
                hitObject.Hit();
            }
            endEffect();
        }
    }

    void endEffect(){
        if(shotVariant == 1){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            Destroy(gameObject);
            Destroy(bulletAnim, .34f);
        }else if(shotVariant == 2){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            //Destroy(gameObject);
            Destroy(bulletAnim, .34f);
        }
    }
}
