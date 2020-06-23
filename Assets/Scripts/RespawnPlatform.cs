using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
   
   public void turnOn(){
        gameObject.transform.GetChild(0).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = .5f;
        gameObject.transform.GetChild(1).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = .5f;
   }

   public void turnOff(){
        gameObject.transform.GetChild(0).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0;
        gameObject.transform.GetChild(1).GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 0;
   }
}
