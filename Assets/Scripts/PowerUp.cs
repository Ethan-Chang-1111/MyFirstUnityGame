using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public int type = 1;
    public float duration = 4;
    bool active = false;

    Collider2D player;
    
   
    void Update(){

        if(active){
            duration -= Time.fixedDeltaTime;  
            if(0 >= duration){
                active = false;
                endEffect();
            } 
        }
    }

     // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D hitInfo){
        if(hitInfo.CompareTag("Player")){//true if hit player
            player = hitInfo;
            active = true;
            startEffect();
        }

    }

    void startEffect(){
        playerWeapon playerScript = player.GetComponent<playerWeapon>();
        playerScript.powerUp(true,type);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    void endEffect(){
        playerWeapon playerScript = player.GetComponent<playerWeapon>();
        playerScript.powerUp(false,type);
        Destroy(gameObject);
    }
}
