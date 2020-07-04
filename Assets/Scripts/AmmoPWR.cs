using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPWR : PowerUp
{
    public int amount = 10;
    public int bulletType = 0;

    public override void Start(){
        type = -1;
        duration = -1;
    }
    public override void startEffect(){
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Pause();
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Clear();
        this.gameObject.transform.GetChild(1).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0;

        weapon.calcAmmo(bulletType,amount);
    }

    public override void endEffect(){
        Destroy(gameObject);
    }
}
