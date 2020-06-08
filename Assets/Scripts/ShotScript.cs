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

        //what other objects do when a bullet hits it
        //only destroys itself when it hits something other than a player or bullet
        if(playerObject == null && shotObject == null){//true if didnt hit player or shot object
            EnemyBehavior hitObject1 = hitInfo.GetComponent<EnemyBehavior>();
            JumperBehavior hitObject2 = hitInfo.GetComponent<JumperBehavior>();
            OctoBehavior hitObject3 = hitInfo.GetComponent<OctoBehavior>();
            if(hitObject1 != null){//true if hit an enemy
                hitObject1.Hit();
            }else if(hitObject2 != null){
                hitObject2.Hit();
            }else if (hitObject3 != null){
                hitObject3.Hit();
            }
            endEffect(hitInfo);
        }
    }


    //what the bullet does when it hits an object
    void endEffect(Collider2D hitInfo){
        if(shotVariant == 1){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            Destroy(gameObject);
            Destroy(bulletAnim, .34f);
        }else if(shotVariant == 2){
            GameObject bulletAnim = Instantiate(impactEffect,transform.position,transform.rotation);
            Destroy(bulletAnim, .34f);
            
            PlatformEffector2D tilemap = hitInfo.GetComponent<PlatformEffector2D>();
            if(tilemap != null){//returns true if hit tilemap
                Destroy(gameObject);
            }
        }
    }
}
