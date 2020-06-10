using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //lowers big shot cooldown

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D hitInfo){
        BasicMove playerObject = hitInfo.GetComponent<BasicMove>();
        playerWeapon weapon = hitInfo.GetComponent<playerWeapon>();
        //what other objects do when a bullet hits it
        //only destroys itself when it hits something other than a player or bullet
        if(playerObject != null){//true if hit player
            playerObject.powerUp();
        }
        if(weapon != null){
            weapon.powerUp(true);
        }

    }
}
